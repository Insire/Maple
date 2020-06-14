using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using MvvmScarletToolkit;
using MvvmScarletToolkit.Observables;

namespace Maple
{
    public sealed class Playlists : ViewModelListBase<Playlist>
    {
        private readonly PlaylistFactory _playlistFactory;

        public ICommand CreateCommand { get; }
        public ICommand UpdateCommand { get; }

        public Func<bool> CanCreateCallback { get; set; }
        public Func<CreatePlaylistViewModel, CancellationToken, bool?> CreateCallback { get; set; }

        public Func<bool> CanUpdateCallback { get; set; }
        public Func<CreatePlaylistViewModel, CancellationToken, bool?> UpdateCallback { get; set; }

        public Playlists(IScarletCommandBuilder commandBuilder, PlaylistFactory playlistFactory)
            : base(commandBuilder)
        {
            _playlistFactory = playlistFactory;

            CreateCommand = CommandBuilder.Create(CreatePlaylist, CanCreatePlaylist)
                .WithAsyncCancellation()
                .WithSingleExecution()
                .Build();

            UpdateCommand = CommandBuilder.Create(Update, CanUpdate)
                .WithAsyncCancellation()
                .WithSingleExecution()
                .Build();
        }

        internal CreatePlaylistViewModel Create()
        {
            return _playlistFactory.Create(this);
        }

        internal CreatePlaylistViewModel Create(Playlist playlist)
        {
            return _playlistFactory.Create(this, playlist);
        }

        private async Task CreatePlaylist(CancellationToken token)
        {
            var dialogViewModel = Create();

            var result = CreateCallback?.Invoke(dialogViewModel, token);
            if (result.HasValue && result.Value)
            {
                await Add(dialogViewModel.Playlist);
            }
        }

        private bool CanCreatePlaylist()
        {
            return !IsBusy && CanCreateCallback?.Invoke() == true;
        }

        private async Task Update(CancellationToken token)
        {
            var oldInstance = SelectedItem;
            var newInstance = new Playlist(SelectedItem);

            var dialogViewModel = Create(newInstance);

            var result = UpdateCallback?.Invoke(dialogViewModel, token);
            if (result.HasValue && result.Value)
            {
                await Add(newInstance);
                await Remove(oldInstance);

                SelectedItem = newInstance;
            }
        }

        private bool CanUpdate()
        {
            return !IsBusy && SelectedItem != null && CanUpdateCallback?.Invoke() == true;
        }
    }
}
