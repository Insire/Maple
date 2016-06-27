using System.Linq;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using InsireBotCore;

namespace InsireBot
{
    /// <summary>
    /// ViewModel that stores and controls which UserControl(Page/View) whatever is displayed in the mainwindow of this app)
    /// </summary>
    public class DrawerItemViewmodel : DefaultViewModelBase<DrawerItem>
    {
        public ICommand SetDrawerItemCommand { get; private set; }

        private object _selectedPage;
        public object SelectedPage
        {
            get { return _selectedPage; }
            set
            {
                _selectedPage = value;
                RaisePropertyChanged(nameof(SelectedPage));
            }
        }

        public DrawerItemViewmodel()
        {
            App.Log.Info("Loading Navigation");

            var content = new[]
            {
                new DrawerItem
                {
                    Content = new MediaPlayerPage(),
                    Name = "Home",
                    Detail = new DrawerItem
                    {
                        Content = new OptionsPage(),
                        Name = "Options"
                    }
                },

                new DrawerItem
                {
                    Content = new NewMediaItemPage(),
                    Name = "Add a Video",
                    Detail = new DrawerItem
                    {
                        Content = new NewMediaItemOptionsPage(),
                        Name = "NewMediaItemOptionsPage"
                    }
                },

                new DrawerItem
                {
                    Content = new NewPlaylistPage(),
                    Name = "Add a Playlist",
                    Detail = new DrawerItem
                    {
                        Content = new NewPlaylistOptionsPage(),
                        Name = "NewPlaylistOptionsPage"
                    }
                },

                new DrawerItem
                {
                    Content = new NewMediaItemPage(),
                    Name = "Discord",
                    Detail = new DrawerItem
                    {
                        Content = new OptionsPage(),
                        Name = "Options"
                    }
                },

                new DrawerItem
                {
                    Content = new NewMediaItemPage(),
                    Name = "Twitch",
                    Detail = new DrawerItem
                    {
                        Content = new OptionsPage(),
                        Name = "Options"
                    }
                },

                new DrawerItem
                {
                    Content = new NewMediaItemPage(),
                    Name = "Log",
                    Detail = new DrawerItem
                    {
                        Content = new OptionsPage(),
                        Name = "Options"
                    }
                },
            };

            Items.AddRange(content);
            SelectedPage = Items.First().Content;
            InitializeCommands();
        }

        private void InitializeCommands()
        {
            SetDrawerItemCommand = new RelayCommand<DrawerItem>((page) =>
            {
                if (page != null)
                    SelectedPage = page.Detail;
            });
        }
    }
}
