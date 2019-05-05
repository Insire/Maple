using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using DryIoc;

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
            var sequenceProvider = ContainerContextExtensions.CreateSequenceService();
            sequenceProvider.Get(default(List<ISequence>)).ReturnsForAnyArgs(5);
            container.UseInstance(sequenceProvider);

            var playlists = (Playlists)container.Resolve<IPlaylistsViewModel>();

            Assert.AreEqual(0, playlists.Count);
            sequenceProvider.ClearReceivedCalls();

            playlists.Add();

            sequenceProvider.Received(1).Get(NSubstitute.Arg.Any<IList<ISequence>>());

            Assert.AreEqual(1, playlists.Count);
            Assert.AreEqual(5, playlists.Items.First().Sequence);
        }

        [TestMethod]
        public async Task Playlists_ShouldAddWithExplicitValue()
        {
            var container = await DependencyInjectionFactory.Get().ConfigureAwait(false);
            var sequenceProvider = ContainerContextExtensions.CreateSequenceService();
            sequenceProvider.Get(default(List<ISequence>)).ReturnsForAnyArgs(5);
            container.UseInstance(sequenceProvider);

            var playlist = container.CreatePlaylist(_context.CreateModelPlaylist());
            var playlists = (Playlists)container.Resolve<IPlaylistsViewModel>();

            Assert.AreEqual(0, playlists.Count);
            sequenceProvider.ClearReceivedCalls();

            playlists.Add(playlist);

            sequenceProvider.Received(1).Get(NSubstitute.Arg.Any<IList<ISequence>>());

            Assert.AreEqual(1, playlists.Count);
            Assert.AreEqual(playlist, playlists.Items.First());
            Assert.AreEqual(5, playlist.Sequence);
        }

        [TestMethod]
        public async Task Playlists_ShouldSave()
        {
            var container = await DependencyInjectionFactory.Get().ConfigureAwait(false);
            var repository = ContainerContextExtensions.CreateRepository();

            container.UseInstance(repository);

            var playlists = container.Resolve<IPlaylistsViewModel>();
            repository.ClearReceivedCalls();

            await playlists.Save().ConfigureAwait(false);

            await repository.Received(1).SaveChanges().ConfigureAwait(false);
            repository.Received(1).Dispose();
        }

        [TestMethod]
        public async Task Playlists_ShouldLoad()
        {
            var container = await DependencyInjectionFactory.Get().ConfigureAwait(false);
            var dummyPlaylists = new List<PlaylistModel>
            {
                _context.CreateModelPlaylist(),
            };
            var repository = ContainerContextExtensions.CreateRepository();
            repository.PlaylistRepository.ReadAsync().ReturnsForAnyArgs(dummyPlaylists);
            container.UseInstance(repository);

            var playlists = (Playlists)container.Resolve<IPlaylistsViewModel>();
            repository.ClearReceivedCalls();

            await playlists.Load().ConfigureAwait(false);
            await repository.PlaylistRepository.Received(1).ReadAsync().ConfigureAwait(false);
            repository.Received(1).Dispose();

            Assert.AreEqual(dummyPlaylists[0], playlists.SelectedItem);
            Assert.AreEqual(1, playlists.Count);
        }
    }
}
