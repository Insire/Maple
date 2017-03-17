using Maple.Core;

namespace Maple
{
    public class ShellViewModel : ObservableObject
    {
        private StatusbarViewModel _statusbarViewModel;
        public StatusbarViewModel StatusbarViewModel
        {
            get { return _statusbarViewModel; }
            private set { SetValue(ref _statusbarViewModel, value); }
        }

        private Scenes _scenes;
        public Scenes Scenes
        {
            get { return _scenes; }
            private set { SetValue(ref _scenes, value); }
        }

        private ITranslationService _translationManager;
        public ITranslationService TranslationManager
        {
            get { return _translationManager; }
            private set { SetValue(ref _translationManager, value); }
        }

        private DialogViewModel _dialogViewModel;
        public DialogViewModel DialogViewModel
        {
            get { return _dialogViewModel; }
            private set { SetValue(ref _dialogViewModel, value); }
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

        private UIColorsViewModel _uiColorsViewModel;
        public UIColorsViewModel UIColorsViewModel
        {
            get { return _uiColorsViewModel; }
            private set { SetValue(ref _uiColorsViewModel, value); }
        }

        private OptionsViewModel _optionsViewModel;
        public OptionsViewModel OptionsViewModel
        {
            get { return _optionsViewModel; }
            private set { SetValue(ref _optionsViewModel, value); }
        }

        public ShellViewModel(ITranslationService translationManager,
                                Scenes scenes,
                                StatusbarViewModel statusBarViewModel,
                                DialogViewModel dialogViewModel,
                                Playlists playlists,
                                MediaPlayers mediaPlayers,
                                UIColorsViewModel uiColorsViewModel,
                                OptionsViewModel optionsViewModel)
        {
            TranslationManager = translationManager;
            Scenes = scenes;
            StatusbarViewModel = statusBarViewModel;
            DialogViewModel = dialogViewModel;
            Playlists = playlists;
            MediaPlayers = mediaPlayers;
            UIColorsViewModel = uiColorsViewModel;
            OptionsViewModel = optionsViewModel;
        }
    }
}
