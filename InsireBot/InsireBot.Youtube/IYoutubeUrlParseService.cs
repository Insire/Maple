using System.Threading.Tasks;

namespace InsireBot.Youtube
{
    public interface IYoutubeUrlParseService
    {
        Task<UrlParseResult> Parse(string data, ParseResultType type);
    }
}
