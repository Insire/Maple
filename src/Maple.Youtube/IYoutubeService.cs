using System.Collections.Generic;
using System.Threading.Tasks;

namespace Maple.Youtube
{
    public interface IYoutubeService
    {
        Task<IEnumerable<YoutubeRessource>> Parse(string data);
    }
}
