using System.Collections.Generic;
using YoutubeExtractor;

namespace Maple.Core
{
    /// <summary>
    ///
    /// </summary>
    public class MrlExtractionService
    {
        /// <summary>
        /// Gets the MRLS.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns></returns>
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
