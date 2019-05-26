using System.Threading.Tasks;
using System.Windows.Input;
using Maple.Youtube;
using MvvmScarletToolkit.Observables;

namespace Maple
{
    /// <summary>
    /// viewmodel for creating playlists from a input string (path/url)
    /// </summary>
    /// <seealso cref="Maple.Core.BaseListViewModel{Maple.Playlist}" />
    public class CreatePlaylist : ViewModelListBase<Playlist>
    {
        private readonly IYoutubeUrlParser _dataParsingService;

        private string _source;
        public string Source
        {
            get { return _source; }
            set { SetValue(ref _source, value); }
        }

        private UrlParseResult _result;
        public UrlParseResult Result
        {
            get { return _result; }
            private set { SetValue(ref _result, value); }
        }

        public ICommand ParseCommand { get; private set; }

        public CreatePlaylist(IYoutubeUrlParser dataParsingService)
            : base(messenger)
        {
            _dataParsingService = dataParsingService;

            ParseCommand = AsyncCommand.Create(Parse, CanParse);
        }

        private async Task Parse()
        {
            using (BusyStack.GetToken())
            {
                Result = await _dataParsingService.Parse(Source, ParseResultType.Playlists)
                                                  .ConfigureAwait(true);

                if (Result.Count > 0 && Result.Playlists?.Count > 0)
                    AddRange(_mapper.GetMany(Result.Playlists));
            }
        }

        private bool CanParse()
        {
            return !string.IsNullOrWhiteSpace(Source);
        }
    }
}
