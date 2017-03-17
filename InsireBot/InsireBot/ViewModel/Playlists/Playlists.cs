using Maple.Core;
using Maple.Localization.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace Maple
{
    public class Playlists : BaseDataListViewModel<Playlist, Data.Playlist>, ILoadableViewModel, ISaveableViewModel
    {
        private readonly IMapleLog _log;
        private readonly DialogViewModel _dialogViewModel;
        private readonly Func<IMediaRepository> _repositoryFactory;

        public ICommand PlayCommand { get; private set; }
        public ICommand LoadCommand => new RelayCommand(Load, () => !IsLoaded);
        public ICommand RefreshCommand => new RelayCommand(Load);
        public ICommand SaveCommand => new RelayCommand(Save);

        public bool IsLoaded { get; private set; }

        public Playlists(IMapleLog log, Func<IMediaRepository> repo, DialogViewModel dialogViewModel)
            : base()
        {
            _dialogViewModel = dialogViewModel;
            _log = log;
            _repositoryFactory = repo;

            AddCommand = new RelayCommand(Add, CanAdd);
        }

        public void Load()
        {
            Items.Clear();

            using (var context = _repositoryFactory())
                Items.AddRange(context.GetAllPlaylists());

            SelectedItem = Items.FirstOrDefault();
            IsLoaded = true;
        }

        public void Save()
        {
            using (var context = _repositoryFactory())
            {
                context.Save(this);
            }
        }

        // TODO order changing + sync, Commands, UserInteraction, async load

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
