using Maple.Core;
using Maple.Localization.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace Maple
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Maple.Core.BaseDataListViewModel{Maple.Playlist, Maple.Data.Playlist}" />
    /// <seealso cref="Maple.Core.ILoadableViewModel" />
    /// <seealso cref="Maple.Core.ISaveableViewModel" />
    public class Playlists : BaseDataListViewModel<Playlist, Data.Playlist>, ILoadableViewModel, ISaveableViewModel
    {
        private readonly IMapleLog _log;
        private readonly DialogViewModel _dialogViewModel;
        private readonly Func<IMediaRepository> _repositoryFactory;

        /// <summary>
        /// Gets the play command.
        /// </summary>
        /// <value>
        /// The play command.
        /// </value>
        public ICommand PlayCommand { get; private set; }
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
        /// Initializes a new instance of the <see cref="Playlists"/> class.
        /// </summary>
        /// <param name="log">The log.</param>
        /// <param name="repo">The repo.</param>
        /// <param name="dialogViewModel">The dialog view model.</param>
        public Playlists(IMapleLog log, Func<IMediaRepository> repo, DialogViewModel dialogViewModel)
            : base()
        {
            _dialogViewModel = dialogViewModel;
            _log = log;
            _repositoryFactory = repo;

            AddCommand = new RelayCommand(Add, CanAdd);
        }

        /// <summary>
        /// Loads this instance.
        /// </summary>
        public void Load()
        {
            Items.Clear();

            using (var context = _repositoryFactory())
                Items.AddRange(context.GetAllPlaylists());

            SelectedItem = Items.FirstOrDefault();
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

        // TODO order changing + sync, Commands, UserInteraction, async load

        /// <summary>
        /// Adds this instance.
        /// </summary>
        public void Add()
        {
            var playlist = new Playlist(_dialogViewModel, new Data.Playlist
            {
                Title = Resources.New,
                Description = string.Empty,
                Location = string.Empty,
                MediaItems = new List<Data.MediaItem>(),
                RepeatMode = 0,
                IsShuffeling = false,
                Sequence = 0,
            });

            if (Count > 0)
            {
                var index = 0;
                var current = Items.Select(p => p.Sequence).ToList();
                while (current.IndexOf(index) >= 0)
                {
                    if (index == int.MaxValue)
                    {
                        _log.Error(Resources.MaxPlaylistCountReachedException);
                        return;
                    }

                    index++;
                }
                playlist.Model.Sequence = index;
            }

            Items.Add(playlist);
        }
    }
}
