using System.Threading.Tasks;
using Maple.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace Maple.Test.ViewModels
{
    [TestClass, TestCategory("PlaylistsTests")]
    public class PlaylistsTests
    {
        private static TestContext _context;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            _context = context;
        }

        [TestMethod]
        public async Task Playlists_ShouldRunConstructorWithErrors()
        {
            var container = await DependencyInjectionFactory.Get().ConfigureAwait(false);

            var playlists = container.CreatePlaylists();

            Assert.AreEqual(false, playlists.IsBusy);
            Assert.AreEqual(false, playlists.IsLoaded);
            Assert.AreEqual(0, playlists.Count);
            Assert.AreEqual(null, playlists.SelectedItem);

            Assert.AreNotEqual(null, playlists.View);
            Assert.AreNotEqual(null, playlists.Items);

            Assert.AreNotEqual(null, playlists.AddCommand);
            Assert.AreNotEqual(null, playlists.ClearCommand);
            Assert.AreNotEqual(null, playlists.LoadCommand);
            Assert.AreNotEqual(null, playlists.RefreshCommand);
            Assert.AreNotEqual(null, playlists.RemoveCommand);
            Assert.AreNotEqual(null, playlists.RemoveRangeCommand);
            Assert.AreNotEqual(null, playlists.SaveCommand);
        }

        [TestMethod]
        public async Task Playlists_ShouldAdd()
        {
            var container = await DependencyInjectionFactory.Get().ConfigureAwait(false);
            var sequenceProvider = ContainerContextExtensions.CreateISequenceService();

            container.UseInstance(sequenceProvider);
            var playlists = container.CreatePlaylists();

            Assert.AreEqual(0, playlists.Count);
            sequenceProvider.ClearReceivedCalls();

            playlists.Add();

            sequenceProvider.Received(1).Get(NSubstitute.Arg.Any<IList<ISequence>>());
            Assert.AreEqual(1, playlists.Count);
        }

        [TestMethod]
        public async Task Playlists_ShouldAddWithExplicitValue()
        {
            var container = await DependencyInjectionFactory.Get().ConfigureAwait(false);
            var playlist = container.CreatePlaylist(_context.CreateModelPlaylist());
            var playlists = container.CreatePlaylists();

            playlists.Add(playlist);

            Assert.Fail();
        }

        [TestMethod]
        public async Task Playlists_ShouldSave()
        {
            var container = await DependencyInjectionFactory.Get().ConfigureAwait(false);

            Assert.Fail();
        }

        [TestMethod]
        public async Task Playlists_ShouldLoad()
        {
            var container = await DependencyInjectionFactory.Get().ConfigureAwait(false);

            Assert.Fail();
        }
    }
}
