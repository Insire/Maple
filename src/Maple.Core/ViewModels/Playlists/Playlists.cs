using System;
using System.Linq;
using System.Threading.Tasks;
using Maple.Domain;
using Maple.Localization.Properties;

namespace Maple.Core
{
    public class Playlists : ValidableBaseDataListViewModel<Playlist, PlaylistModel, int>, ISaveableViewModel, IPlaylistsViewModel
    {
        private readonly Func<IMediaRepository> _repositoryFactory;
        private readonly IPlaylistMapper _playlistMapper;

        public Playlists(ViewModelServiceContainer container, IPlaylistMapper playlistMapper, Func<IMediaRepository> repositoryFactory)
            : base(container)
        {
            _repositoryFactory = repositoryFactory ?? throw new ArgumentNullException(nameof(repositoryFactory), $"{nameof(repositoryFactory)} {Resources.IsRequired}");
            _playlistMapper = playlistMapper ?? throw new ArgumentNullException(nameof(playlistMapper), $"{nameof(playlistMapper)} {Resources.IsRequired}");

            AddCommand = new RelayCommand(Add, CanAdd);
        }

        public void Add()
        {
            Add(_playlistMapper.GetNewPlaylist());
        }

        public bool CanAdd()
        {
            return Items != null;
        }

        public override async Task SaveAsync()
        {
            _log.Info($"{_translationService.Translate(nameof(Resources.Saving))} {_translationService.Translate(nameof(Resources.Playlists))}");
            using (var context = _repositoryFactory())
            {
                await context.SaveAsync(this).ConfigureAwait(true);
            }
        }

        public override async Task GetCountAsync()
        {
            _log.Info($"{_translationService.Translate(nameof(Resources.Loading))} {_translationService.Translate(nameof(Resources.Playlists))}");
            Clear();

            using (var context = _repositoryFactory())
            {
                var result = await context.GetPlaylistsAsync().ConfigureAwait(true);
                AddRange(result);
            }

            SelectedItem = Items.FirstOrDefault();
            IsLoaded = true;
        }
    }
}
