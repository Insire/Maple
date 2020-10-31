using SoftThorn.MonstercatNet;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Maple
{
    public static class MonstercatApiExtensions
    {
        public static async Task<MonstercatRessourceViewModel> Parse(this IMonstercatApi api, string input, CancellationToken token)
        {
            if (!input.Contains("monstercat.com", StringComparison.InvariantCultureIgnoreCase))
            {
                return null;
            }

            if (Uri.TryCreate(input.Trim(), UriKind.RelativeOrAbsolute, out var url))
            {
                var collection = HttpUtility.ParseQueryString(url.Query);
                var keys = collection.AllKeys;

                foreach (var key in keys)
                {
                    var id = collection.Get(key).ToLowerInvariant();

                    switch (key)
                    {
                        case "release":
                            var result = await api.GetRelease(id);
                            if (result?.Release is null)
                            {
                                return null;
                            }

                            return new MonstercatReleaseViewModel(result.Release);

                        case "playlist":

                            if (Guid.TryParse(id, out var guid))
                            {
                                var tracks = await api.GetPlaylistTracks(guid);
                            }

                            throw new NotImplementedException();
                    }
                }
            }

            return null;
        }
    }
}
