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

        private ITranslationManager _translationManager;
        public ITranslationManager TranslationManager
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

        public ShellViewModel(ITranslationManager translationManager, Scenes scenes, StatusbarViewModel statusBarViewModel, DialogViewModel dialogViewModel)
        {
            TranslationManager = translationManager;
            Scenes = scenes;
            StatusbarViewModel = statusBarViewModel;
            DialogViewModel = dialogViewModel;
        }
    }
}
