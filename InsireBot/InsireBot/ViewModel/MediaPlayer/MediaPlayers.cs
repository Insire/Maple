using Maple.Core;
using System;
using System.Windows.Input;

namespace Maple
{
    public class MediaPlayers : BaseDataListViewModel<MediaPlayer, Data.MediaPlayer>, IDisposable, ILoadableViewModel, ISaveableViewModel
    {
        private readonly ITranslationService _manager;
        private readonly Func<IMediaPlayer> _playerFactory;
        private readonly AudioDevices _devices;
        private readonly DialogViewModel _dialog;
        private readonly Func<IMediaRepository> _repositoryFactory;

        private bool _disposed;
        public bool Disposed
        {
            get { return _disposed; }
            protected set { SetValue(ref _disposed, value); }
        }

        public ICommand LoadCommand => new RelayCommand(Load, () => !IsLoaded);
        public ICommand RefreshCommand => new RelayCommand(Load);
        public ICommand SaveCommand => new RelayCommand(Save);

        public bool IsLoaded { get; private set; }

        public MediaPlayers(ITranslationService manager, Func<IMediaPlayer> playerFactory, Func<IMediaRepository> repo, AudioDevices devices, DialogViewModel dialog)
        {
            _manager = manager;
            _playerFactory = playerFactory;
            _devices = devices;
            _dialog = dialog;
            _repositoryFactory = repo;
        }

        public void Load()
        {
            Items.Clear();

            using (var context = _repositoryFactory())
            {
                var main = context.GetMainMediaPlayer();

                Items.Add(main);
                SelectedItem = main;

                Items.AddRange(context.GetAllOptionalMediaPlayers());
            }

            IsLoaded = true;
        }

        public void Save()
        {
            using (var context = _repositoryFactory())
            {
                context.Save(this);
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Dispose(bool disposing)
        {
            if (Disposed)
                return;

            if (disposing)
            {
                foreach (var player in Items)
                    player.Dispose();

                // Free any other managed objects here.
            }

            // Free any unmanaged objects here.
            Disposed = true;
        }
    }
}
