using System;
using System.Linq;
using InsireBotCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace InsireBotTests
{
    [TestClass]
    public class PlaylistUnitTests
    {
        [TestMethod]
        public void InitializePlaylistTest()
        {
            var service = new TestDataService();
            var items = service.GetMediaItems();
            var playlist = new Playlist<IMediaItem>(service.GetMediaItems());

            var first = items.First();
            Assert.AreEqual(playlist.CurrentItem.Location, first.Location);
            var second = items.ElementAt(1);

            Assert.AreEqual(playlist.Next().Location, second.Location);
            Assert.AreEqual(playlist.Next().Location, second.Location);
        }

        [TestMethod]
        public void PlaylistUpdateTest()
        {
            var service = new TestDataService();
            var items = service.GetMediaItems();
            var playlist = new Playlist<IMediaItem>(service.GetMediaItems());

            var first = items.First();
            Assert.AreEqual(playlist.CurrentItem.Location, first.Location);
            var second = items.ElementAt(1);
            var next = playlist.Next();
            Assert.AreEqual(next.Location, second.Location);

            playlist.Set(next);
            Assert.AreEqual(next.Location, playlist.CurrentItem.Location);
        }

        [TestMethod]
        public void PlaylistPlaymodeNoneTest()
        {
            var service = new TestDataService();
            var items = service.GetMediaItems();
            var playlist = new Playlist<IMediaItem>(service.GetMediaItems());

            var first = items.First();
            Assert.AreEqual(playlist.CurrentItem.Location, first.Location);
            var second = items.ElementAt(1);
            var next = playlist.Next();
            Assert.AreEqual(next.Location, second.Location);

            playlist.Set(next);
            Assert.AreEqual(next.Location, playlist.CurrentItem.Location);
        }
    }
}
