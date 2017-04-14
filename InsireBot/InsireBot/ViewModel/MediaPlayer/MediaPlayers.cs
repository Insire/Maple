using Maple.Core;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Maple
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="Maple.Core.BaseDataListViewModel{Maple.MediaPlayer, Maple.Data.MediaPlayer}" />
    /// <seealso cref="System.IDisposable" />
    /// <seealso cref="Maple.Core.ILoadableViewModel" />
    /// <seealso cref="Maple.Core.ISaveableViewModel" />
    public class MediaPlayers : BaseDataListViewModel<MediaPlayer, Data.MediaPlayer>, IDisposable, ILoadableViewModel, ISaveableViewModel
    {
        private readonly ITranslationService _manager;
        private readonly Func<IMediaPlayer> _playerFactory;
        private readonly AudioDevices _devices;
        private readonly DialogViewModel _dialog;
        private readonly Func<IMediaRepository> _repositoryFactory;

        private bool _disposed;
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="MediaPlayers"/> is disposed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if disposed; otherwise, <c>false</c>.
        /// </value>
        public bool Disposed
        {
            get { return _disposed; }
            protected set { SetValue(ref _disposed, value); }
        }

        /// <summary>
        /// Gets the load command.
        /// </summary>
        /// <value>
        /// The load command.
        /// </value>
        public ICommand LoadCommand => new RelayCommand(Load, () => !IsLoaded);
        /// <summary>
        /// Gets the refresh command.
        /// </summary>
        /// <value>
        /// The refresh command.
        /// </value>
        public ICommand RefreshCommand => new RelayCommand(Load);
        /// <summary>
        /// Gets the save command.
        /// </summary>
        /// <value>
        /// The save command.
        /// </value>
        public ICommand SaveCommand => new RelayCommand(Save);

        /// <summary>
        /// Gets a value indicating whether this instance is loaded.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is loaded; otherwise, <c>false</c>.
        /// </value>
        public bool IsLoaded { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MediaPlayers"/> class.
        /// </summary>
        /// <param name="manager">The manager.</param>
        /// <param name="playerFactory">The player factory.</param>
        /// <param name="repo">The repo.</param>
        /// <param name="devices">The devices.</param>
        /// <param name="dialog">The dialog.</param>
        public MediaPlayers(ITranslationService manager, Func<IMediaPlayer> playerFactory, Func<IMediaRepository> repo, AudioDevices devices, DialogViewModel dialog)
        {
            _manager = manager;
            _playerFactory = playerFactory;
            _devices = devices;
            _dialog = dialog;
            _repositoryFactory = repo;
        }

        /// <summary>
        /// Loads this instance.
        /// </summary>
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

        /// <summary>
        /// Saves this instance.
        /// </summary>
        public void Save()
        {
            using (var context = _repositoryFactory())
            {
                context.Save(this);
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (Disposed)
                return;

            if (disposing)
            {
                foreach (var player in Items)
                    player?.Dispose();

                // Free any other managed objects here.
            }

            // Free any unmanaged objects here.
            Disposed = true;
        }

        public Task SaveAsync()
        {
            return Task.Run(() => Save());
        }

        public async Task LoadAsync()
        {
            Items.Clear();

            using (var context = _repositoryFactory())
            {
                var main = await Task.Run(() => context.GetMainMediaPlayer());

                Items.Add(main);
                SelectedItem = main;
                var others = await Task.Run(() => context.GetAllOptionalMediaPlayers());
                Items.AddRange(others);
            }

            IsLoaded = true;
        }
    }
}
