using Maple.Core;

namespace Maple
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Maple.Core.ObservableObject" />
    public class ShellViewModel : ObservableObject
    {
        private StatusbarViewModel _statusbarViewModel;
        /// <summary>
        /// Gets the statusbar view model.
        /// </summary>
        /// <value>
        /// The statusbar view model.
        /// </value>
        public StatusbarViewModel StatusbarViewModel
        {
            get { return _statusbarViewModel; }
            private set { SetValue(ref _statusbarViewModel, value); }
        }

        private Scenes _scenes;
        /// <summary>
        /// Gets the scenes.
        /// </summary>
        /// <value>
        /// The scenes.
        /// </value>
        public Scenes Scenes
        {
            get { return _scenes; }
            private set { SetValue(ref _scenes, value); }
        }

        private ITranslationService _translationManager;
        /// <summary>
        /// Gets the translation manager.
        /// </summary>
        /// <value>
        /// The translation manager.
        /// </value>
        public ITranslationService TranslationManager
        {
            get { return _translationManager; }
            private set { SetValue(ref _translationManager, value); }
        }

        private DialogViewModel _dialogViewModel;
        /// <summary>
        /// Gets the dialog view model.
        /// </summary>
        /// <value>
        /// The dialog view model.
        /// </value>
        public DialogViewModel DialogViewModel
        {
            get { return _dialogViewModel; }
            private set { SetValue(ref _dialogViewModel, value); }
        }

        private Playlists _playlists;
        /// <summary>
        /// Gets the playlists.
        /// </summary>
        /// <value>
        /// The playlists.
        /// </value>
        public Playlists Playlists
        {
            get { return _playlists; }
            private set { SetValue(ref _playlists, value); }
        }

        private MediaPlayers _mediaPlayers;
        /// <summary>
        /// Gets the media players.
        /// </summary>
        /// <value>
        /// The media players.
        /// </value>
        public MediaPlayers MediaPlayers
        {
            get { return _mediaPlayers; }
            private set { SetValue(ref _mediaPlayers, value); }
        }

        private UIColorsViewModel _uiColorsViewModel;
        /// <summary>
        /// Gets the UI colors view model.
        /// </summary>
        /// <value>
        /// The UI colors view model.
        /// </value>
        public UIColorsViewModel UIColorsViewModel
        {
            get { return _uiColorsViewModel; }
            private set { SetValue(ref _uiColorsViewModel, value); }
        }

        private OptionsViewModel _optionsViewModel;
        /// <summary>
        /// Gets the options view model.
        /// </summary>
        /// <value>
        /// The options view model.
        /// </value>
        public OptionsViewModel OptionsViewModel
        {
            get { return _optionsViewModel; }
            private set { SetValue(ref _optionsViewModel, value); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShellViewModel"/> class.
        /// </summary>
        /// <param name="translationManager">The translation manager.</param>
        /// <param name="scenes">The scenes.</param>
        /// <param name="statusBarViewModel">The status bar view model.</param>
        /// <param name="dialogViewModel">The dialog view model.</param>
        /// <param name="playlists">The playlists.</param>
        /// <param name="mediaPlayers">The media players.</param>
        /// <param name="uiColorsViewModel">The UI colors view model.</param>
        /// <param name="optionsViewModel">The options view model.</param>
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
