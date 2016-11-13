using System.Linq;
using InsireBotCore;

namespace InsireBot
{
    /// <summary>
    /// ViewModel that stores and controls which UserControl(Page/View) whatever is displayed in the mainwindow of this app)
    /// </summary>
    public class DrawerItemViewmodel : DefaultViewModelBase<DrawerItem>
    {
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

                    GetDataContext = ()=>GlobalServiceLocator.Instance.MediaPlayerViewModel,
                },

                new DrawerItem
                {
                    Content = new ColorOptionsPage(),
                    Name = "Color Options",
                    GetDataContext =()=>GlobalServiceLocator.Instance.UIColorsViewModel,
                },

                new DrawerItem
                {
                    Content = new NewMediaItemOptionsPage(),
                    Name = "NewMediaItemOptionsPage"
                },

                new DrawerItem
                {
                    Content = new NewMediaItemPage(),
                    Name = "Add a Video",
                    GetDataContext =()=>GlobalServiceLocator.Instance.CreateMediaItemViewModel,
                },

                new DrawerItem
                {
                    Content = new NewPlaylistOptionsPage(),
                    Name = "NewPlaylistOptionsPage"
                },

                new DrawerItem
                {
                    Content = new NewPlaylistPage(),
                    Name = "Add a Playlist",
                },

                new DrawerItem
                {
                    Content = new OptionsPage(),
                    Name = "Options"
                },

                new DrawerItem
                {
                    Content = new NewMediaItemPage(),
                    Name = "Discord",
                },

                new DrawerItem
                {
                    Content = new NewMediaItemPage(),
                    Name = "Twitch",
                },

                new DrawerItem
                {
                    Content = new NewMediaItemPage(),
                    Name = "Log",
                },
            };

            Items.AddRange(content);
            SelectedPage = Items.First().Content;
        }
    }
}
