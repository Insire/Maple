using System.Threading.Tasks;

namespace Maple.Youtube
{
    public interface IYoutubeUrlParseService
    {
        Task<UrlParseResult> Parse(string data, ParseResultType type);
    }
}
