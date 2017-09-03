using System;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using Maple.Interfaces;
using Maple.Localization.Properties;

namespace Maple.Youtube
{
    public class YoutubeUrlParser : IYoutubeUrlParser
    {
        private const string _pattern = @"(?:http|https|)(?::\/\/|)(?:www.|m.|)(?:youtu\.be\/|youtube\.com(?:\/embed\/|\/v\/|\/watch\?v=|\|\/feeds\/api\/videos\/|\/user\S*[^\w\-\s]|\S*[^\w\-\s]))([\w\-]*)[a-z0-9;:@?&%=+\/\$_.-]*";

        private readonly ILoggingService _log;
        private readonly YoutubeApi _api;
        private readonly Regex _urlPattern;

        public YoutubeUrlParser(ILoggingService log)
        {
            _log = log ?? throw new ArgumentNullException(nameof(log), $"{nameof(log)} {Resources.IsRequired}");

            _urlPattern = new Regex(_pattern, RegexOptions.Compiled);
            _api = new YoutubeApi(_log);
        }

        public async Task<UrlParseResult> Parse(string data, ParseResultType type)
        {
            var failure = new UrlParseResult(_log, ParseResultType.None);

            if (type == ParseResultType.None)
                return failure;

            if (string.IsNullOrEmpty(data))
                return failure;

            if (Uri.TryCreate(data.Trim(), UriKind.RelativeOrAbsolute, out var url))
            {
                try
                {
                    switch (url.DnsSafeHost)
                    {
                        case "youtu.be":
                            url = new Uri(url.AbsoluteUri.Replace(@"youtu.be/", @"youtube.com/watch?v="));
                            return await Parse(url, type).ConfigureAwait(false);

                        case "www.youtube.com":
                            return await Parse(url, type).ConfigureAwait(false);
                    }
                }
                catch (Exception ex)
                {
                    _log.Error(this, ex);
                }
            }

            return failure;
        }

        private async Task<UrlParseResult> Parse(Uri url, ParseResultType type)
        {
            var result = new UrlParseResult(_log, type);
            var match = _urlPattern.Match(url.AbsoluteUri);
            var collection = default(NameValueCollection);

            while (match.Success)
            {
                collection = HttpUtility.ParseQueryString(url.Query);
                var keys = collection.AllKeys;

                foreach (var key in keys)
                {
                    var id = collection.Get(key).ToLowerInvariant();

                    switch (key)
                    {
                        case "v":
                            var videos = await _api.GetVideo(id).ConfigureAwait(false);

                            foreach (var video in videos)
                                result.MediaItems.Add(video);

                            break;

                        case "list":
                            var playlists = await _api.GetPlaylists(id).ConfigureAwait(false);

                            foreach (var playlist in playlists)
                                result.Playlists.Add(playlist);

                            break;

                        default:
                            return new UrlParseResult(_log, ParseResultType.None);
                    }
                }

                match = match.NextMatch();
            }

            return result;
        }
    }
}
