using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bogus;
using Maple.Domain;

namespace Maple.Data
{
    public sealed class DesignTimeDataFactory
    {
        private readonly IUnitOfWork _unitOfWork;

        public DesignTimeDataFactory(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task SeedDatabase()
        {
            Randomizer.Seed = new Random(8757858);
            const string localization = "de";

            var mediaItemGenerator = new Faker<MediaItemModel>(localization)
                .RuleFor(o => o.Id, f => f.IndexFaker + 1)
                .RuleFor(o => o.Duration, f => f.Random.Long())
                .RuleFor(o => o.Sequence, f => f.IndexFaker)
                .RuleFor(o => o.Title, f => f.Lorem.Random.Word())
                .RuleFor(o => o.Location, f => f.Lorem.Random.Word());

            var mediaPlayerGenerator = new Faker<MediaPlayerModel>(localization)
                .RuleFor(o => o.Id, f => f.IndexFaker + 1)
                .RuleFor(o => o.DeviceName, f => f.Lorem.Random.Words())
                .RuleFor(o => o.Sequence, f => f.IndexFaker)
                .RuleFor(o => o.Name, f => f.Lorem.Random.Word());

            var playlistGenerator = new Faker<PlaylistModel>(localization)
                .RuleFor(o => o.Id, f => f.IndexFaker + 1)
                .RuleFor(o => o.Sequence, f => f.IndexFaker)
                .RuleFor(o => o.Title, f => f.Lorem.Random.Word())
                .RuleFor(o => o.MediaItems, (f, o) =>
                {
                    var mediaItems = new List<MediaItemModel>();
                    for (var i = 0; i < f.Random.Int(max: 20); i++)
                    {
                        var mediaItem = mediaItemGenerator.Generate();
                        mediaItem.Playlist = o;
                        mediaItems.Add(mediaItem);
                    }

                    return mediaItems;
                })
                .RuleFor(o => o.MediaPlayer, f =>
                {
                    return mediaPlayerGenerator.Generate();
                });

            var count = Randomizer.Seed.Next(20, 100);

            for (var i = 0; i < count; i++)
            {
                var playlist = playlistGenerator.Generate();
                _unitOfWork.Playlists.Create(playlist);
            }

            await _unitOfWork.SaveChanges();
        }
    }
}
