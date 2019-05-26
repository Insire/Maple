using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Maple.Core;
using Maple.Domain;
using Maple.Localization.Properties;

namespace Maple
{
    public sealed class MediaPlayers : MapleDomainViewModelListBase<MediaPlayer, MediaPlayerModel>
    {
        private readonly Func<IUnitOfWork> _repositoryFactory;

        public Playlists Playlists { get; }

        public MediaPlayers(IMapleCommandBuilder commandBuilder, IValidator<MediaPlayer> validator, Playlists playlists)
            : base(commandBuilder, validator)
        {
            Playlists = playlists ?? throw new ArgumentNullException(nameof(playlists));
        }

        protected override async Task RefreshInternal(CancellationToken token)
        {
            NotificationService.Info($"{LocalizationService.Translate(nameof(Resources.Loading))} {LocalizationService.Translate(nameof(Resources.MediaPlayers))}");

            //    using (var context = _repositoryFactory())
            //    {
            //        var main = await context.MediaPlayerRepository.GetMainMediaPlayerAsync().ConfigureAwait(true);
            //        if (!(main is null))
            //        {
            //            var viewModel = _mediaPlayerMapper.Get(main);
            //            Add(viewModel);
            //            SelectedItem = viewModel;
            //        }
            //        else
            //        {
            //            main = new MediaPlayerModel()
            //            {
            //                IsPrimary = true,
            //                Name = "Primary",
            //                Sequence = SequenceService.Get(Items.Cast<ISequence>().ToList()),
            //            };

            //            if (!Playlists.IsLoaded)
            //            {
            //                await Playlists.Load();
            //            }

            //            if (Playlists.Count > 0)
            //            {
            //                var viewModel = _mediaPlayerMapper.GetMain(main, Playlists[0]);
            //                await Add(viewModel);
            //                SelectedItem = viewModel;
            //            }
            //            else
            //            {
            //                await Playlists.Add();
            //                var viewModel = _mediaPlayerMapper.GetMain(main, Playlists[0]);
            //                Add(viewModel);
            //                SelectedItem = viewModel;
            //            }
            //        }

            //        var others = await context.MediaPlayerRepository.GetOptionalMediaPlayersAsync().ConfigureAwait(true);
            //        if (others.Count > 0)
            //        {
            //            AddRange(_mediaPlayerMapper.GetMany(others));
            //        }
            //    }
        }
    }
}
