using InsireBot.Localization.Properties;
using MvvmScarletToolkit;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;

namespace InsireBot
{
    /// <summary>
    /// ViewModel that stores and controls which UserControl(Page/View) whatever is displayed in the mainwindow of this app)
    /// </summary>
    public class Scenes : ViewModelBase<Scene>
    {
        private ITranslationManager _manager;

        public ICommand OpenColorOptionsCommand { get; private set; }
        public ICommand OpenMediaPlayerCommand { get; private set; }
        public ICommand OpenGithubPageCommand { get; private set; }

        public Scenes(ITranslationManager manager,
                        MediaPlayerViewModel mediaPlayerViewModel,
                        CreateMediaItemViewModel createMediaItemViewModel,
                        PlaylistsViewModel playlistsViewModel,
                        CreatePlaylistViewModel createPlaylistViewModel,
                        UIColorsViewModel uIColorsViewModel,
                        OptionsViewModel optionsViewModel)
        {
            _manager = manager;
            var mediaPlayer = mediaPlayerViewModel;

            App.Log.Info(_manager.Translate(nameof(Resources.NavigationLoad)));

            var content = new[]
            {
                new Scene
                {
                    Content = new MediaPlayerPage(_manager),
                    DisplayName =_manager.Translate(nameof(Resources.Playback)),
                    GetDataContext = () => mediaPlayerViewModel,
                    IsSelected = true,
                },

                new Scene
                {
                    Content = new NewMediaItemPage(_manager),
                    DisplayName = _manager.Translate(nameof(Resources.VideoAdd)),
                    GetDataContext = () => mediaPlayer,
                    IsSelected = false,
                },

                new Scene
                {
                    Content = new PlaylistsPage(_manager),
                    DisplayName = _manager.Translate(nameof(Resources.Playlists)),
                    GetDataContext =() => playlistsViewModel,
                    IsSelected = false,
                },

                new Scene
                {
                    Content = new NewPlaylistPage(_manager),
                    DisplayName = _manager.Translate(nameof(Resources.PlaylistAdd)),
                    GetDataContext =() => createPlaylistViewModel,
                    IsSelected = false,
                },

                new Scene
                {
                    Content = new ColorOptionsPage(_manager),
                    DisplayName = _manager.Translate(nameof(Resources.Themes)),
                    GetDataContext =() => uIColorsViewModel,
                    IsSelected = false,
                },

                new Scene
                {
                    Content = new OptionsPage(_manager),
                    DisplayName = _manager.Translate(nameof(Resources.Options)),
                    GetDataContext =() => optionsViewModel,
                    IsSelected = false,
                },
            };

            using (BusyStack.GetToken())
            {
                AddRange(content);

                using (View.DeferRefresh())
                {
                    View.SortDescriptions.Add(new SortDescription(nameof(Scene.DisplayName), ListSortDirection.Ascending));
                }
            }

            InitializeCommands();

            App.Log.Info(manager.Translate(nameof(Resources.NavigationLoaded)));
        }

        private void InitializeCommands()
        {
            OpenColorOptionsCommand = new RelayCommand(OpenColorOptionsView, CanOpenColorOptionsView);
            OpenMediaPlayerCommand = new RelayCommand(OpenMediaPlayerView, CanOpenMediaPlayerView);
            OpenGithubPageCommand = new RelayCommand(OpenGithubPage);
        }

        private void OpenColorOptionsView()
        {
            SelectedItem = Items.First(p => p.Content.GetType() == typeof(ColorOptionsPage));
        }

        private bool CanOpenColorOptionsView()
        {
            return Items?.Any(p => p.Content.GetType() == typeof(ColorOptionsPage)) == true;
        }

        private void OpenMediaPlayerView()
        {
            SelectedItem = Items.First(p => p.Content.GetType() == typeof(MediaPlayerPage));
        }

        private bool CanOpenMediaPlayerView()
        {
            return Items?.Any(p => p.Content.GetType() == typeof(ColorOptionsPage)) == true;
        }

        private void OpenGithubPage()
        {
            Process.Start(_manager.Translate(nameof(Resources.GithubProjectLink)));
        }
    }
}
