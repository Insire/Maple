using System.Threading.Tasks;
using Maple.Data;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Maple.UnitTests
{
    [TestFixture, Category("IntegrationTest")]
    public class DatabaseTests
    {
        [Test]
        public async Task ShouldSeedDatabase()
        {
            using (var context = new PlaylistContext(new DbContextOptionsBuilder<PlaylistContext>()
                        .UseInMemoryDatabase("DatabaseTests.ShouldSeedDatabase")
                        .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                        .Options))
            {
                var unitOfWork = new MapleUnitOfWork(context);
                var factory = new DesignTimeDataFactory(unitOfWork);
                await factory.SeedDatabase();

                var mediaItems = await unitOfWork.MediaItems.ReadAsync();

                Assert.NotNull(mediaItems);
                Assert.IsTrue(mediaItems.Count > 0);

                var playlists = await unitOfWork.Playlists.ReadAsync();

                Assert.NotNull(playlists);
                Assert.IsTrue(playlists.Count > 0);

                var mediaPlayers = await unitOfWork.MediaPlayers.ReadAsync();

                Assert.NotNull(mediaPlayers);
                Assert.IsTrue(mediaPlayers.Count > 0);
            }
        }
    }
}
