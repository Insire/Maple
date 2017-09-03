using Maple.Core;
using Maple.Localization.Properties;
using Maple.Youtube;
using System;
using System.Threading.Tasks;
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

        public CreateMediaItem(IYoutubeUrlParseService dataParsingService, IMediaItemMapper mapper, IMessenger messenger)
            : base(messenger)
        {
            _dataParsingService = dataParsingService ?? throw new ArgumentNullException(nameof(dataParsingService), $"{nameof(mapper)} {Resources.IsRequired}");
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper), $"{nameof(mapper)} {Resources.IsRequired}");

            InitializeCommands();
        }

        private void InitializeCommands()
        {
            ParseCommand = new AsyncRelayCommand(Parse, CanParse);
        }

        private async Task Parse()
        {
            Result = await _dataParsingService.Parse(Source, ParseResultType.MediaItems)
                                              .ConfigureAwait(true);

            if (Result.Count > 0 && Result.MediaItems?.Count > 0)
                Items.AddRange(_mapper.GetMany(Result.MediaItems));
        }

        private bool CanParse()
        {
            return !string.IsNullOrWhiteSpace(Source);
        }
    }
}
