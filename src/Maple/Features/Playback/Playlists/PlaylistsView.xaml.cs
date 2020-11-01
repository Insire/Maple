using System.Threading;
using System.Windows;
using MvvmScarletToolkit;

namespace Maple
{
    public partial class PlaylistsView
    {
        private Shell _shell;
        private Playlists _playlists;

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

            _playlists = playlists;

            _playlists.CanCreateCallback += CanCreate;
            _playlists.CreateCallback += Create;

            _playlists.CanUpdateCallback += CanUpdate;
            _playlists.UpdateCallback += Update;
        }

        private void PlaylistsView_Unloaded(object sender, RoutedEventArgs e)
        {
            if (_playlists is null)
            {
                return;
            }

            _playlists.CreateCallback -= Create;
            _playlists.CanCreateCallback -= CanCreate;

            _playlists.UpdateCallback -= Update;
            _playlists.CanUpdateCallback -= CanUpdate;
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
