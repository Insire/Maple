using Maple.Core;
using Maple.Youtube;
using System.Windows.Input;

namespace Maple
{
    /// <summary>
    /// viewmodel for creating playlists from a input string (path/url)
    /// </summary>
    public class CreatePlaylistViewModel : BaseListViewModel<Playlist>
    {
        private IYoutubeUrlParseService _dataParsingService;
        private IPlaylistMapper _mapper;

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

        public CreatePlaylistViewModel(IYoutubeUrlParseService dataParsingService, IPlaylistMapper mapper) : base()
        {
            _dataParsingService = dataParsingService;
            _mapper = mapper;

            InitializeCommands();
        }

        private void InitializeCommands()
        {
            ParseCommand = new RelayCommand(async () =>
            {
                using (BusyStack.GetToken())
                {
                    Result = await _dataParsingService.Parse(Source, ParseResultType.Playlists);

                    if (Result.Count > 0 && Result.Playlists?.Count > 0)
                        Items.AddRange(_mapper.GetMany(Result.Playlists));
                }
            }, () => !string.IsNullOrWhiteSpace(Source));
        }
    }
}
