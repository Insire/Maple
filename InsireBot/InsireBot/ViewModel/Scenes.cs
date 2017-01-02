using MvvmScarletToolkit;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace InsireBotWPF
{
    /// <summary>
    /// ViewModel that stores and controls which UserControl(Page/View) whatever is displayed in the mainwindow of this app)
    /// </summary>
    public class Scenes : ViewModelBase<Scene>
    {
        public ICommand OpenColorOptionsCommand { get; private set; }
        public ICommand OpenMediaPlayerCommand { get; private set; }

        public Scenes()
        {
            App.Log.Info("Loading Navigation");

            var content = new[]
            {
                new Scene
                {
                    Content = new MediaPlayerPage(),
                    DisplayName = "Playback",
                    GetDataContext = () => GlobalServiceLocator.Instance.MediaPlayerViewModel,
                    IsSelected = true,
                },

                new Scene
                {
                    Content = new ColorOptionsPage(),
                    DisplayName = "Themes",
                    GetDataContext =() => GlobalServiceLocator.Instance.UIColorsViewModel,
                    IsSelected = false,
                },

                new Scene
                {
                    Content = new NewMediaItemPage(),
                    DisplayName = "Add Video",
                    GetDataContext =() => GlobalServiceLocator.Instance.CreateMediaItemViewModel,
                    IsSelected = false,
                },

                new Scene
                {
                    Content = new NewPlaylistPage(),
                    DisplayName = "Add Playlist",
                    GetDataContext =() => GlobalServiceLocator.Instance.CreatePlaylistViewModel,
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

            App.Log.Info("Loaded Navigation");
        }

        private void InitializeCommands()
        {
            OpenColorOptionsCommand = new RelayCommand(OpenColorOptionsView, CanOpenColorOptionsView);
            OpenMediaPlayerCommand = new RelayCommand(OpenMediaPlayerView, CanOpenMediaPlayerView);
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
    }
}
