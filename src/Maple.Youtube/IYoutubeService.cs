using System.Collections.Generic;
using System.Threading.Tasks;

namespace Maple.Youtube
{
    public interface IYoutubeService
    {
        IEnumerable<string> GetMrls(string url);
        Task<IEnumerable<YoutubeRessource>> Parse(string data);
    }
}
