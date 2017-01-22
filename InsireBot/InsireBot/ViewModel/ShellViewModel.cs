using InsireBot.Core;

namespace InsireBot
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

        private ITranslationManager _translationManager;
        public ITranslationManager TranslationManager
        {
            get { return _translationManager; }
            private set { SetValue(ref _translationManager, value); }
        }

        public ShellViewModel(ITranslationManager translationManager, Scenes scenes, StatusbarViewModel statusBarViewModel)
        {
            TranslationManager = translationManager;
            Scenes = scenes;
            StatusbarViewModel = statusBarViewModel;
        }
    }
}
