using System;
using System.Linq;
using System.Threading.Tasks;
using Maple.Core;
using Maple.Domain;
using Maple.Localization.Properties;

namespace Maple
{
    public class Playlists : BaseDataListViewModel<Playlist, PlaylistModel>, ISaveableViewModel, IPlaylistsViewModel
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

        private void SaveInternal()
        {
            _log.Info($"{_translationService.Translate(nameof(Resources.Saving))} {_translationService.Translate(nameof(Resources.Playlists))}");
            using (var context = _repositoryFactory())
            {
                context.Save(this);
            }
        }

        public void Add()
        {
            Add(_playlistMapper.GetNewPlaylist());
        }

        public bool CanAdd()
        {
            return Items != null;
        }

        public override void Save()
        {
            SaveInternal();
        }

        public override async Task LoadAsync()
        {
            _log.Info($"{_translationService.Translate(nameof(Resources.Loading))} {_translationService.Translate(nameof(Resources.Playlists))}");
            Clear();

            using (var context = _repositoryFactory())
            {
                var result = await context.GetPlaylistsAsync().ConfigureAwait(true);
                AddRange(result);
            }

            SelectedItem = Items.FirstOrDefault();
            OnLoaded();
        }
    }
}
