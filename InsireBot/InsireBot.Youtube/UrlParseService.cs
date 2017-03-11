using Maple.Core;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace Maple.Youtube
{
    public class UrlParseService : IYoutubeUrlParseService
    {
        private IMapleLog _log;
        public UrlParseService(IMapleLog log)
        {
            _log = log;
        }

        public async Task<UrlParseResult> Parse(string data, ParseResultType type)
        {
            var url = new Uri(data.Trim());
            try
            {
                switch (url.DnsSafeHost)
                {
                    case "youtu.be":
                        url = new Uri(url.AbsoluteUri.Replace(@"youtu.be/", @"youtube.com/watch?v="));
                        return await Parse(url, type);

                    case "www.youtube.com":
                        return await Parse(url, type);

                    default:
                        return new UrlParseResult(_log);
                }
            }
            catch (Exception ex)
            {
                _log.Error(this, ex);
                return new UrlParseResult(_log);
            }
        }

        private async Task<UrlParseResult> Parse(Uri url, ParseResultType type = ParseResultType.None)
        {
            var result = new UrlParseResult(_log, type);
            var regex = @"(?:http|https|)(?::\/\/|)(?:www.|m.|)(?:youtu\.be\/|youtube\.com(?:\/embed\/|\/v\/|\/watch\?v=|\|\/feeds\/api\/videos\/|\/user\S*[^\w\-\s]|\S*[^\w\-\s]))([\w\-]*)[a-z0-9;:@?&%=+\/\$_.-]*";
            var match = Regex.Match(url.AbsoluteUri, regex);
            var youtubeService = new Service(_log);

            while (match.Success)
            {
                var keys = HttpUtility.ParseQueryString(url.Query).AllKeys;

                foreach (var key in keys)
                {
                    var id = HttpUtility.ParseQueryString(url.Query).Get(key);

                    switch (key)
                    {
                        case "v":
                            var videos = await youtubeService.GetVideo(id);

                            foreach (var video in videos)
                            {
                                result.MediaItems.Add(video);
                            }

                            break;

                        case "list":
                            var playlists = await youtubeService.GetPlaylists(id);

                            foreach (var playlist in playlists)
                            {
                                result.Playlists.Add(playlist);
                            }

                            break;
                    }
                }

                match = match.NextMatch();
            }

            return result;
        }
    }
}
