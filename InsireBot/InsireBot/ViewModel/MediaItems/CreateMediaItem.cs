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

        /// <summary>
        /// Gets the parse command.
        /// </summary>
        /// <value>
        /// The parse command.
        /// </value>
        public ICommand ParseCommand { get; private set; }

        private string _source;
        /// <summary>
        /// Gets or sets the source.
        /// </summary>
        /// <value>
        /// The source.
        /// </value>
        public string Source
        {
            get { return _source; }
            set { SetValue(ref _source, value); }
        }

        private UrlParseResult _result;
        /// <summary>
        /// Gets the result.
        /// </summary>
        /// <value>
        /// The result.
        /// </value>
        public UrlParseResult Result
        {
            get { return _result; }
            private set { SetValue(ref _result, value); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateMediaItem"/> class.
        /// </summary>
        /// <param name="dataParsingService">The data parsing service.</param>
        /// <param name="mapper">The mapper.</param>
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
