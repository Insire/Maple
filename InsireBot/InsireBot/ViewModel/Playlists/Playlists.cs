using Maple.Core;
using Maple.Data;
using Maple.Localization.Properties;
using System.Linq;
using System.Windows.Input;

namespace Maple
{
    public class Playlists : BaseListViewModel<Playlist>
    {
        private readonly IPlaylistContext _context;
        private readonly IBotLog _log;
        private readonly DialogViewModel _dialogViewModel;

        public ICommand PlayCommand { get; private set; }

        public Playlists(IPlaylistContext context, IBotLog log, DialogViewModel dialogViewModel) : base()
        {
            _dialogViewModel = dialogViewModel;
            _log = log;


            Items.AddRange(context.Playlists.Select(p => new Playlist(_dialogViewModel, p)));
            SelectedItem = Items.FirstOrDefault();

            AddCommand = new RelayCommand(Add, CanAdd);
        }

        // TODO order changing + sync, Commands, UserInteraction, async load

        public void Add()
        {
            var playlist = Data.Playlist.New();

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
                playlist.Sequence = index;
            }

            Items.Add(new Playlist(_dialogViewModel, playlist));
        }
    }
}
