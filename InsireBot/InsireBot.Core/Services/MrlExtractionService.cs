using System.Collections.Generic;
using YoutubeExtractor;

namespace Maple.Core
{
    public class MrlExtractionService
    {
        public static IEnumerable<string> GetMrls(string url)
        {
            var normalizedUrl = string.Empty;

            if (DownloadUrlResolver.TryNormalizeYoutubeUrl(url, out normalizedUrl))
            {
                var videoInfos = DownloadUrlResolver.GetDownloadUrls(normalizedUrl);

                foreach (var item in videoInfos)
                    yield return item.DownloadUrl;
            }
        }
    }
}
