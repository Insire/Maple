using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using MvvmScarletToolkit;

namespace Maple
{
    public partial class PlaylistsView
    {
        public ICommand CreateCommand
        {
            get { return (ICommand)GetValue(CreateCommandProperty); }
            set { SetValue(CreateCommandProperty, value); }
        }

        public static readonly DependencyProperty CreateCommandProperty = DependencyProperty.Register(
            nameof(CreateCommand),
            typeof(ICommand),
            typeof(PlaylistsView),
            new PropertyMetadata(default(ICommand)));

        public ICommand UpdateCommand
        {
            get { return (ICommand)GetValue(UpdateCommandProperty); }
            set { SetValue(UpdateCommandProperty, value); }
        }

        public static readonly DependencyProperty UpdateCommandProperty = DependencyProperty.Register(
            nameof(UpdateCommand),
            typeof(ICommand),
            typeof(PlaylistsView),
            new PropertyMetadata(default(ICommand)));

        private Shell _shell;

        public PlaylistsView()
        {
            InitializeComponent();
        }

        private void PlaylistsView_Loaded(object sender, RoutedEventArgs e)
        {
            _shell = this.FindParent<Shell>();

            CreateCommand = _shell.CommandBuilder.Create(Create, CanCreate)
                .WithAsyncCancellation()
                .WithSingleExecution()
                .Build();

            UpdateCommand = _shell.CommandBuilder.Create(Update, CanUpdate)
                .WithAsyncCancellation()
                .WithSingleExecution()
                .Build();
        }

        private async Task Create(CancellationToken token)
        {
            if (!(DataContext is Playlists mediaPlayers))
            {
                return;
            }

            if (_shell is null)
            {
                return;
            }

            var viewModel = mediaPlayers.Create();

            var dlg = new CreatePlaylistDialog(viewModel, _shell, token);

            var result = dlg.ShowDialog();
            if (result.HasValue && result.Value)
            {
                await mediaPlayers.Add(viewModel.Playlist);
            }
        }

        private bool CanCreate()
        {
            return _shell != null && DataContext is Playlists;
        }

        private async Task Update(CancellationToken token)
        {
            if (!(DataContext is Playlists itemsViewModel))
            {
                return;
            }

            if (_shell is null)
            {
                return;
            }

            var oldInstance = itemsViewModel.SelectedItem;
            var newInstance = new Playlist(itemsViewModel.SelectedItem);

            var dialogViewModel = itemsViewModel.Create(newInstance);
            var dlg = new CreatePlaylistDialog(dialogViewModel, _shell, token);

            var result = dlg.ShowDialog();
            if (result.HasValue && result.Value)
            {
                await itemsViewModel.Add(newInstance);
                await itemsViewModel.Remove(oldInstance);

                itemsViewModel.SelectedItem = newInstance;
            }
        }

        private bool CanUpdate()
        {
            return _shell != null && DataContext is Playlists itemsViewModel && itemsViewModel.SelectedItem != null;
        }
    }
}
