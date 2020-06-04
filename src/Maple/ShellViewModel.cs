using System;
using System.Threading;
using System.Threading.Tasks;
using MvvmScarletToolkit;
using MvvmScarletToolkit.Observables;

namespace Maple
{
    internal sealed class ShellViewModel : BusinessViewModelBase
    {
        private MetaDataViewModel _metaDataViewModel;
        public MetaDataViewModel MetaDataViewModel
        {
            get { return _metaDataViewModel; }
            private set { SetValue(ref _metaDataViewModel, value); }
        }

        private NavigationViewModel _navigationViewModel;
        public NavigationViewModel NavigationViewModel
        {
            get { return _navigationViewModel; }
            private set { SetValue(ref _navigationViewModel, value); }
        }

        private LocalizationsViewModel _localizations;
        public LocalizationsViewModel Localizations
        {
            get { return _localizations; }
            private set { SetValue(ref _localizations, value); }
        }

        private Playlists _playlists;
        public Playlists Playlists
        {
            get { return _playlists; }
            private set { SetValue(ref _playlists, value); }
        }

        private MediaPlayers _mediaPlayers;
        public MediaPlayers MediaPlayers
        {
            get { return _mediaPlayers; }
            private set { SetValue(ref _mediaPlayers, value); }
        }

        private OptionsViewModel _optionsViewModel;
        public OptionsViewModel OptionsViewModel
        {
            get { return _optionsViewModel; }
            private set { SetValue(ref _optionsViewModel, value); }
        }

        public ShellViewModel(IScarletCommandBuilder commandBuilder,
                                LocalizationsViewModel localizationsViewModel,
                                NavigationViewModel navigationViewModel,
                                MetaDataViewModel metaDataViewModel,
                                Playlists playlists,
                                MediaPlayers mediaPlayers,
                                OptionsViewModel optionsViewModel)
            : base(commandBuilder)
        {
            Localizations = localizationsViewModel ?? throw new ArgumentNullException(nameof(localizationsViewModel));
            NavigationViewModel = navigationViewModel ?? throw new ArgumentNullException(nameof(navigationViewModel));
            MetaDataViewModel = metaDataViewModel ?? throw new ArgumentNullException(nameof(metaDataViewModel));
            Playlists = playlists ?? throw new ArgumentNullException(nameof(playlists));
            MediaPlayers = mediaPlayers ?? throw new ArgumentNullException(nameof(mediaPlayers));
            OptionsViewModel = optionsViewModel ?? throw new ArgumentNullException(nameof(optionsViewModel));
        }

        protected override Task UnloadInternal(CancellationToken token)
        {
            // serialize all media players first, thgen serialize playlists

            return Task.CompletedTask;
        }

        protected override Task RefreshInternal(CancellationToken token)
        {
            // all audio devices
            // all playlists
            // all playlist contents
            // current/ last mediaplayer
            // current /last culture

            // deserialize all media players first, thgen deserialize playlists

            return Task.CompletedTask;
        }
    }
}
