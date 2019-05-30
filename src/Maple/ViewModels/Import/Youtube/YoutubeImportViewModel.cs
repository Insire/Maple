using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Maple.Youtube;
using MvvmScarletToolkit.Abstractions;
using MvvmScarletToolkit.Commands;
using MvvmScarletToolkit.Observables;

namespace Maple
{
    public sealed class YoutubeImportViewModel : ViewModelListBase<YoutubeRessourceViewModel>
    {
        private readonly IYoutubeService _youtubeService;

        private string _source;
        public string Source
        {
            get { return _source; }
            set { SetValue(ref _source, value); }
        }

        public ICommand ParseCommand { get; }

        public YoutubeImportViewModel(ICommandBuilder commandBuilder, IYoutubeService youtubeService)
            : base(commandBuilder)
        {
            _youtubeService = youtubeService ?? throw new ArgumentNullException(nameof(youtubeService));

            ParseCommand = commandBuilder
                .Create(Parse, CanParse)
                .WithSingleExecution(CommandManager)
                .Build();
        }

        private async Task Parse(CancellationToken token)
        {
            using (BusyStack.GetToken())
            {
                var results = await _youtubeService
                    .Parse(Source)
                    .ConfigureAwait(true);

                foreach (var model in results)
                {
                    switch (model)
                    {
                        case YoutubePlaylist playlist:
                            await Add(new YoutubePlaylistViewModel(playlist)).ConfigureAwait(false);
                            break;

                        case YoutubeVideo video:
                            await Add(new YoutubeVideoViewModel(video)).ConfigureAwait(false);
                            break;
                    }
                }
            }
        }

        private bool CanParse()
        {
            return !IsBusy && !string.IsNullOrWhiteSpace(Source);
        }
    }
}
