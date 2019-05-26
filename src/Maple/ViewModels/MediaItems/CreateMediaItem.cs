using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Maple.Domain;
using Maple.Localization.Properties;
using Maple.Youtube;
using MvvmScarletToolkit.Abstractions;
using MvvmScarletToolkit.Commands;
using MvvmScarletToolkit.Observables;

namespace Maple
{
    /// <summary>
    /// viewmodel for creating mediaitem from a string (path/url)
    /// </summary>
    public class CreateMediaItem : ViewModelListBase<MediaItem>
    {
        private IYoutubeUrlParser _dataParsingService;

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

        public CreateMediaItem(IYoutubeUrlParser dataParsingService, ICommandBuilder builder)
            : base(builder)
        {
            _dataParsingService = dataParsingService ?? throw new ArgumentNullException(nameof(dataParsingService));

            InitializeCommands();
        }

        private void InitializeCommands()
        {
            ParseCommand = CommandBuilder.Create(Parse, CanParse).Build();
        }

        private async Task Parse()
        {
            Result = await _dataParsingService.Parse(Source, ParseResultType.MediaItems)
                                              .ConfigureAwait(true);

            if (Result.Count > 0 && Result.MediaItems?.Count > 0)
                await AddRange(_mapper.GetMany(Result.MediaItems)).ConfigureAwait(false);
        }

        private bool CanParse()
        {
            return !string.IsNullOrWhiteSpace(Source);
        }

        public MediaItem Create(string absolutePath)
        {
            var model = new MediaItemModel()
            {
                Location = absolutePath,
                MediaItemType = (int)MediaItemType.LocalFile,
                PrivacyStatus = (int)PrivacyStatus.None,
                Title = Path.GetFileName(absolutePath),
            };

            return _mapper.Get(model);
        }

        protected override Task Load(CancellationToken token)
        {
            throw new NotImplementedException();
        }

        protected override Task Unload(CancellationToken token)
        {
            throw new NotImplementedException();
        }

        protected override Task Refresh(CancellationToken token)
        {
            throw new NotImplementedException();
        }
    }
}
