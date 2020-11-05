using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace Maple.Youtube
{
    public class YoutubeService : IYoutubeService
    {
        private const string YoutubeUrlPattern = @"(?:http|https|)(?::\/\/|)(?:www.|m.|)(?:youtu\.be\/|youtube\.com(?:\/embed\/|\/v\/|\/watch\?v=|\|\/feeds\/api\/videos\/|\/user\S*[^\w\-\s]|\S*[^\w\-\s]))([\w\-]*)[a-z0-9;:@?&%=+\/\$_.-]*";
        private const string YoutubeDomain = "www.youtube.com";
        private const string YoutuDomain = "youtu.be";

        private readonly YoutubeApi _api;
        private readonly Regex _urlPattern;

        public YoutubeService()
        {
            _urlPattern = new Regex(YoutubeUrlPattern, RegexOptions.Compiled);
            _api = new YoutubeApi();
        }

        public IEnumerable<string> GetMrls(string url)
        {
            return Enumerable.Empty<string>();
        }

        public async Task<IEnumerable<YoutubeRessource>> Parse(string data)
        {
            if (string.IsNullOrEmpty(data))
                return Enumerable.Empty<YoutubeRessource>();

            if (Uri.TryCreate(data.Trim(), UriKind.RelativeOrAbsolute, out var url))
            {
                switch (url.DnsSafeHost)
                {
                    case YoutuDomain:
                        url = new Uri(url.AbsoluteUri.Replace(@"youtu.be/", @"youtube.com/watch?v="));
                        return await Parse(url).ConfigureAwait(false);

                    case YoutubeDomain:
                        return await Parse(url).ConfigureAwait(false);
                }
            }

            return Enumerable.Empty<YoutubeRessource>();
        }

        private async Task<ICollection<YoutubeRessource>> Parse(Uri url)
        {
            var result = new List<YoutubeRessource>();
            var match = _urlPattern.Match(url.AbsoluteUri);

            while (match.Success)
            {
                var collection = HttpUtility.ParseQueryString(url.Query);
                var keys = collection.AllKeys;

                foreach (var key in keys)
                {
                    var id = collection.Get(key).ToLowerInvariant();

                    switch (key)
                    {
                        case "v":
                            var videos = await _api.GetVideo(id).ConfigureAwait(false);
                            result.AddRange(videos);

                            break;

                        case "list":
                            var playlists = await _api.GetPlaylists(id).ConfigureAwait(false);
                            result.AddRange(playlists);

                            break;
                    }
                }

                match = match.NextMatch();
            }

            return result;
        }
    }
}
