using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using MvvmScarletToolkit;

namespace Maple
{
    public partial class MediaPlayersView
    {
        public ICommand CreateCommand
        {
            get { return (ICommand)GetValue(CreateCommandProperty); }
            set { SetValue(CreateCommandProperty, value); }
        }

        public static readonly DependencyProperty CreateCommandProperty = DependencyProperty.Register(
            nameof(CreateCommand),
            typeof(ICommand),
            typeof(MediaPlayersView),
            new PropertyMetadata(default(ICommand)));

        public ICommand UpdateCommand
        {
            get { return (ICommand)GetValue(UpdateCommandProperty); }
            set { SetValue(UpdateCommandProperty, value); }
        }

        public static readonly DependencyProperty UpdateCommandProperty = DependencyProperty.Register(
            nameof(UpdateCommand),
            typeof(ICommand),
            typeof(MediaPlayersView),
            new PropertyMetadata(default(ICommand)));

        public ICommand AddFromFileCommand
        {
            get { return (ICommand)GetValue(AddFromFileCommandProperty); }
            set { SetValue(AddFromFileCommandProperty, value); }
        }

        public static readonly DependencyProperty AddFromFileCommandProperty = DependencyProperty.Register(
            nameof(AddFromFileCommand),
            typeof(ICommand),
            typeof(MediaPlayersView),
            new PropertyMetadata(default(ICommand)));

        public ICommand AddFromUrlCommand
        {
            get { return (ICommand)GetValue(AddFromUrlCommandProperty); }
            set { SetValue(AddFromUrlCommandProperty, value); }
        }

        public static readonly DependencyProperty AddFromUrlCommandProperty = DependencyProperty.Register(
            nameof(AddFromUrlCommand),
            typeof(ICommand),
            typeof(MediaPlayersView),
            new PropertyMetadata(default(ICommand)));

        public ICommand AddFromFolderCommand
        {
            get { return (ICommand)GetValue(AddFromFolderCommandProperty); }
            set { SetValue(AddFromFolderCommandProperty, value); }
        }

        public static readonly DependencyProperty AddFromFolderCommandProperty = DependencyProperty.Register(
            nameof(AddFromFolderCommand),
            typeof(ICommand),
            typeof(MediaPlayersView),
            new PropertyMetadata(default(ICommand)));

        private Shell _shell;

        public MediaPlayersView()
        {
            InitializeComponent();
        }

        private void MediaPlayersView_Loaded(object sender, RoutedEventArgs e)
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
            if (!(DataContext is MediaPlayers mediaPlayers))
            {
                return;
            }

            if (_shell is null)
            {
                return;
            }

            var viewModel = mediaPlayers.Create();

            var dlg = new CreateMediaPlayerDialog(viewModel, _shell, token);

            var result = dlg.ShowDialog();
            if (result.HasValue && result.Value)
            {
                await mediaPlayers.Add(viewModel.MediaPlayer);
            }
        }

        private bool CanCreate()
        {
            return _shell != null && DataContext is MediaPlayers;
        }

        private async Task Update(CancellationToken token)
        {
            if (!(DataContext is MediaPlayers mediaPlayers))
            {
                return;
            }

            if (_shell is null)
            {
                return;
            }

            var oldInstance = mediaPlayers.SelectedItem;
            var newInstance = new MediaPlayer(mediaPlayers.SelectedItem);

            var dialogViewModel = mediaPlayers.Create(newInstance);
            var dlg = new CreateMediaPlayerDialog(dialogViewModel, _shell, token);

            var result = dlg.ShowDialog();
            if (result.HasValue && result.Value)
            {
                await mediaPlayers.Add(newInstance);
                await mediaPlayers.Remove(oldInstance);

                mediaPlayers.SelectedItem = newInstance;
            }
        }

        private bool CanUpdate()
        {
            return _shell != null && DataContext is MediaPlayers mediaPlayers && mediaPlayers.SelectedItem != null;
        }
    }
}
