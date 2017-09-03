using System.Threading.Tasks;

namespace Maple.Youtube
{
    public interface IYoutubeUrlParser
    {
        Task<UrlParseResult> Parse(string data, ParseResultType type);
    }
}
