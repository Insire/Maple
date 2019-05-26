using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Maple.Core;
using Maple.Domain;
using Maple.Localization.Properties;

namespace Maple
{
    public sealed class Playlists : MapleBusinessViewModelListBase<Playlist, PlaylistModel>
    {
        private readonly Func<IUnitOfWork> _repositoryFactory;

        public Playlists(IMapleCommandBuilder commandBuilder)
            : base(commandBuilder)
        {
        }

        private async Task SaveInternal()
        {
            _log.Info($"{LocalizationService.Translate(nameof(Resources.Saving))} {LocalizationService.Translate(nameof(Resources.Playlists))}");
            using (var context = _repositoryFactory())
            {
                foreach (var item in Items)
                    context.PlaylistRepository.Update(item.Model);

                await context.SaveChanges().ConfigureAwait(false);
            }
        }

        protected override Task RefreshInternal(CancellationToken token)
        {
            Log.Info($"{LocalizationService.Translate(nameof(Resources.Loading))} {LocalizationService.Translate(nameof(Resources.Playlists))}");

            using (var context = _repositoryFactory())
            {
                var result = await context.PlaylistRepository.ReadAsync(null, new[] { nameof(PlaylistModel.MediaItems) }, -1, -1).ConfigureAwait(true);

                if (result.Count > 0)
                {
                    AddRange(_playlistMapper.GetMany(result));
                }
                else
                {
                    await Add();
                }
            }

            SelectedItem = Items.FirstOrDefault();
        }
    }
}
