using Maple.Core;
using Maple.Youtube;
using System.Windows.Input;

namespace Maple
{
    /// <summary>
    /// viewmodel for creating mediaitem from a string (path/url)
    /// </summary>
    public class CreateMediaItem : BaseListViewModel<IMediaItem>
    {
        private IYoutubeUrlParseService _dataParsingService;
        private IMediaItemMapper _mapper;

        public ICommand ParseCommand { get; private set; }

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

        public CreateMediaItem(IYoutubeUrlParseService dataParsingService, IMediaItemMapper mapper) : base()
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
                    Result = await _dataParsingService.Parse(Source, ParseResultType.MediaItems);

                    if (Result.Count > 0 && Result.MediaItems?.Count > 0)
                        Items.AddRange(_mapper.GetMany(Result.MediaItems));
                }
            }, () => !string.IsNullOrWhiteSpace(Source));
        }
    }
}
