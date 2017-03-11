using Maple.Core;
using Maple.Data;
using Maple.Localization.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Maple
{
    public class Playlists : BaseDataListViewModel<Playlist, Data.Playlist>, IRefreshable
    {
        private readonly IMapleLog _log;
        private readonly DialogViewModel _dialogViewModel;

        public ICommand PlayCommand { get; private set; }

        public Playlists(IMapleLog log, DialogViewModel dialogViewModel)
            : base()
        {
            _dialogViewModel = dialogViewModel;
            _log = log;

            AddCommand = new RelayCommand(Add, CanAdd);
        }

        public Task LoadAsync()
        {
            return Task.Run(() =>
            {
                Items.Clear();

                using (var context = new PlaylistContext())
                {
                    foreach (var item in context.Playlists)
                        Items.Add(new Playlist(_dialogViewModel, item));

                    SelectedItem = Items.FirstOrDefault();
                }
            });
        }

        public Task SaveAsync()
        {
            throw new NotImplementedException();
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
