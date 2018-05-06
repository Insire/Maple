﻿using System.Threading.Tasks;
using System.Windows.Input;
using Maple.Youtube;

namespace Maple.Core
{
    /// <summary>
    /// viewmodel for creating playlists from a input string (path/url)
    /// </summary>
    /// <seealso cref="Maple.Core.BaseListViewModel{Maple.Playlist}" />
    public class CreatePlaylist : BaseListViewModel<Playlist>
    {
        private readonly IYoutubeUrlParser _dataParsingService;
        private readonly IPlaylistMapper _mapper;

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
        /// Gets the parse command.
        /// </summary>
        /// <value>
        /// The parse command.
        /// </value>
        public ICommand ParseCommand { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CreatePlaylist"/> class.
        /// </summary>
        /// <param name="dataParsingService">The data parsing service.</param>
        /// <param name="mapper">The mapper.</param>
        public CreatePlaylist(IYoutubeUrlParser dataParsingService, IPlaylistMapper mapper, IMessenger messenger)
            : base(messenger)
        {
            _dataParsingService = dataParsingService;
            _mapper = mapper;

            InitializeCommands();
        }

        private void InitializeCommands()
        {
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
