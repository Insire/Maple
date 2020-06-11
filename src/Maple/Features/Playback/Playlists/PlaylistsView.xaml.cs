using System.Threading;
using System.Windows;
using MvvmScarletToolkit;

namespace Maple
{
    public partial class PlaylistsView
    {
        private Shell _shell;

        public PlaylistsView()
        {
            InitializeComponent();
        }

        private void PlaylistsView_Loaded(object sender, RoutedEventArgs e)
        {
            _shell = this.FindParent<Shell>();

            if (_shell is null)
            {
                return;
            }

            if (!(DataContext is Playlists playlists))
            {
                return;
            }

            playlists.CanCreateCallback += CanCreate;
            playlists.CreateCallback += Create;

            playlists.CanUpdateCallback += CanUpdate;
            playlists.UpdateCallback += Update;
        }

        private void PlaylistsView_Unloaded(object sender, RoutedEventArgs e)
        {
            if (!(DataContext is Playlists playlists))
            {
                return;
            }

            playlists.CreateCallback -= Create;
            playlists.CanCreateCallback -= CanCreate;

            playlists.UpdateCallback -= Update;
            playlists.CanUpdateCallback -= CanUpdate;
        }

        private bool? Create(CreatePlaylistViewModel viewModel, CancellationToken token)
        {
            var dlg = new CreatePlaylistDialog(viewModel, _shell, token);

            return dlg.ShowDialog();
        }

        private bool CanCreate()
        {
            return _shell != null;
        }

        private bool? Update(CreatePlaylistViewModel viewModel, CancellationToken token)
        {
            var dlg = new CreatePlaylistDialog(viewModel, _shell, token);

            return dlg.ShowDialog();
        }

        private bool CanUpdate()
        {
            return _shell != null;
        }
    }
}
