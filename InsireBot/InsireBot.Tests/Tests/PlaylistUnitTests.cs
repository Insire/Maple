using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace InsireBot.Tests
{
    [TestClass]
    public class PlaylistUnitTests
    {
        [TestMethod]
        public void InitializePlaylistTest()
        {
            var service = new TestDataService();
            var playlist = new Playlist(service.GetMediaItems());

            var first = playlist.First();
            var second = playlist.ElementAt(1);

            Assert.AreEqual(RepeatMode.None, playlist.RepeatMode);
            Assert.AreEqual(false, playlist.IsShuffeling);
            Assert.AreEqual(false, playlist.IsSelected);
            Assert.AreEqual(playlist.CurrentItem.Location, first.Location);
            Assert.AreEqual(playlist.Next().Location, second.Location);
            Assert.AreEqual(playlist.Next().Location, second.Location);
        }

        [TestMethod]
        public void PlaylistUpdateTest()
        {
            var service = new TestDataService();
            var playlist = new Playlist(service.GetMediaItems());

            var first = playlist.First();
            Assert.AreEqual(playlist.CurrentItem.Location, first.Location);
            var second = playlist.ElementAt(1);
            var next = playlist.Next();
            Assert.AreEqual(next.Location, second.Location);

            playlist.Set(next);
            Assert.AreEqual(next.Location, playlist.CurrentItem.Location);
        }

        [TestMethod]
        public void PlaylistRepeatModeChangeTest()
        {
            var eventFired = false;
            var service = new TestDataService();
            var playlist = new Playlist(service.GetMediaItems());
            var first = playlist.First();
            var second = playlist.ElementAt(1);


            playlist.RepeatModeChanged += (o, e) =>
            {
                eventFired = true;
            };

            playlist.RepeatMode = RepeatMode.None;
            Assert.AreEqual(RepeatMode.None, playlist.RepeatMode);
            Assert.AreEqual(false, eventFired);
            eventFired = false;

            playlist.RepeatMode = RepeatMode.All;
            Assert.AreEqual(RepeatMode.All, playlist.RepeatMode);
            Assert.AreEqual(true, eventFired);
            eventFired = false;

            playlist.RepeatMode = RepeatMode.Single;
            Assert.AreEqual(RepeatMode.Single, playlist.RepeatMode);
            Assert.AreEqual(true, eventFired);
            eventFired = false;
        }

        [TestMethod]
        public void PlaylistRepeatModeNoneTest()
        {
            var service = new TestDataService();
            var playlist = new Playlist(service.GetMediaItems());
            var first = playlist.ElementAt(0);
            var second = playlist.ElementAt(1);
            var third = playlist.ElementAt(2);
            var fourth = playlist.ElementAt(3);

            Assert.AreEqual(playlist.CurrentItem.Location, first.Location);

            var next = playlist.Next();
            Assert.AreEqual(next.Location, second.Location);
            Assert.AreEqual(first.Location, playlist.CurrentItem.Location);

            playlist.Set(next);
            Assert.AreEqual(next.Location, playlist.CurrentItem.Location);
            Assert.AreEqual(second.Location, playlist.CurrentItem.Location);

            next = playlist.Next();
            playlist.Set(next);
            Assert.AreEqual(next.Location, playlist.CurrentItem.Location);
            Assert.AreEqual(third.Location, playlist.CurrentItem.Location);

            next = playlist.Next();
            playlist.Set(next);
            Assert.AreEqual(next.Location, playlist.CurrentItem.Location);
            Assert.AreEqual(fourth.Location, playlist.CurrentItem.Location);

            next = playlist.Next();
            playlist.Set(next);
            Assert.AreEqual(next, null);
            Assert.AreEqual(fourth.Location, playlist.CurrentItem.Location);
        }

        [TestMethod]
        public void PlaylistRepeatModeSingleTest()
        {
            var service = new TestDataService();
            var playlist = new Playlist(service.GetMediaItems());
            var first = playlist.ElementAt(0);
            var second = playlist.ElementAt(1);
            var third = playlist.ElementAt(2);
            var fourth = playlist.ElementAt(3);

            Assert.AreEqual(playlist.CurrentItem.Location, first.Location);

            var next = playlist.Next();
            Assert.AreEqual(next.Location, second.Location);
            Assert.AreEqual(first.Location, playlist.CurrentItem.Location);

            next = playlist.Next();
            Assert.AreEqual(next.Location, second.Location);
            Assert.AreEqual(first.Location, playlist.CurrentItem.Location);
        }

        [TestMethod]
        public void PlaylistRepeatModeSinglePreviousTest()
        {
            var service = new TestDataService();
            var playlist = new Playlist(service.GetMediaItems());
            var first = playlist.ElementAt(0);
            var second = playlist.ElementAt(1);
            var third = playlist.ElementAt(2);
            var fourth = playlist.ElementAt(3);

            Assert.AreEqual(playlist.CurrentItem.Location, first.Location);

            var next = playlist.Next();
            Assert.AreEqual(next.Location, second.Location);
            Assert.AreEqual(first.Location, playlist.CurrentItem.Location);

            next = playlist.Next();
            Assert.AreEqual(next.Location, second.Location);
            Assert.AreEqual(first.Location, playlist.CurrentItem.Location);
        }

        public void PlaylistShuffleTest()
        {
            var service = new TestDataService();
            var playlist = new Playlist(service.GetMediaItems());
            var first = playlist.ElementAt(0);
            var second = playlist.ElementAt(1);
            var third = playlist.ElementAt(2);
            var fourth = playlist.ElementAt(3);

            Assert.AreEqual(playlist.CurrentItem.Location, first.Location);

            var next = playlist.Next();
            Assert.AreEqual(next.Location, second.Location);

            next = playlist.Next();
            Assert.AreEqual(next.Location, second.Location);
            Assert.AreEqual(first.Location, playlist.CurrentItem.Location);
        }

        [TestMethod]
        public void PlaylistRepeatModeAllTest()
        {
            var service = new TestDataService();
            var playlist = new Playlist(service.GetMediaItems());
            var first = playlist.ElementAt(0);
            var second = playlist.ElementAt(1);
            var third = playlist.ElementAt(2);
            var fourth = playlist.ElementAt(3);

            playlist.RepeatMode = RepeatMode.All;

            Assert.AreEqual(playlist.CurrentItem.Location, first.Location);

            var next = playlist.Next();
            Assert.AreEqual(next.Location, second.Location);
            Assert.AreEqual(first.Location, playlist.CurrentItem.Location);

            playlist.Set(next);
            Assert.AreEqual(next.Location, playlist.CurrentItem.Location);
            Assert.AreEqual(second.Location, playlist.CurrentItem.Location);

            next = playlist.Next();
            playlist.Set(next);
            Assert.AreEqual(next.Location, playlist.CurrentItem.Location);
            Assert.AreEqual(third.Location, playlist.CurrentItem.Location);

            next = playlist.Next();
            playlist.Set(next);
            Assert.AreEqual(next.Location, playlist.CurrentItem.Location);
            Assert.AreEqual(fourth.Location, playlist.CurrentItem.Location);

            next = playlist.Next();
            playlist.Set(next);
            Assert.AreEqual(next.Location, playlist.CurrentItem.Location);
            Assert.AreEqual(first.Location, playlist.CurrentItem.Location);

            next = playlist.Next();
            playlist.Set(next);
            Assert.AreEqual(next.Location, playlist.CurrentItem.Location);
            Assert.AreEqual(second.Location, playlist.CurrentItem.Location);
        }

        [TestMethod]
        public void PlaylistIndexTest()
        {
            var service = new TestDataService();
            var playlist = new Playlist(service.GetMediaItems());
            Assert.AreEqual(true, playlist.All(p => p.Sequence > -1));
            Assert.AreEqual(true, playlist.GroupBy(p => p.Sequence).All(p => p.Count() == 1));
        }

        [TestMethod]
        public void PlaylistRepeatModeAllPreviousTest()
        {
            var service = new TestDataService();
            var playlist = new Playlist(service.GetMediaItems());
            var first = playlist.ElementAt(0);
            var second = playlist.ElementAt(1);
            var third = playlist.ElementAt(2);
            var fourth = playlist.ElementAt(3);

            playlist.RepeatMode = RepeatMode.All;

            Assert.AreEqual(playlist.CurrentItem.Location, first.Location);

            var next = playlist.Next();
            Assert.AreEqual(next.Location, second.Location);
            Assert.AreEqual(first.Location, playlist.CurrentItem.Location);

            playlist.Set(next);
            Assert.AreEqual(next.Location, playlist.CurrentItem.Location);
            Assert.AreEqual(second.Location, playlist.CurrentItem.Location);

            next = playlist.Next();
            playlist.Set(next);
            Assert.AreEqual(next.Location, playlist.CurrentItem.Location);
            Assert.AreEqual(third.Location, playlist.CurrentItem.Location);

            next = playlist.Next();
            playlist.Set(next);
            Assert.AreEqual(next.Location, playlist.CurrentItem.Location);
            Assert.AreEqual(fourth.Location, playlist.CurrentItem.Location);

            next = playlist.Next();
            playlist.Set(next);
            Assert.AreEqual(next.Location, playlist.CurrentItem.Location);
            Assert.AreEqual(first.Location, playlist.CurrentItem.Location);

            next = playlist.Next();
            playlist.Set(next);
            Assert.AreEqual(next.Location, playlist.CurrentItem.Location);
            Assert.AreEqual(second.Location, playlist.CurrentItem.Location);

            var previous = playlist.Previous();
            playlist.Set(previous);
            Assert.AreEqual(previous.Location, playlist.CurrentItem.Location);
            Assert.AreEqual(first.Location, playlist.CurrentItem.Location);

            previous = playlist.Previous();
            playlist.Set(previous);
            Assert.AreEqual(previous.Location, playlist.CurrentItem.Location);
            Assert.AreEqual(fourth.Location, playlist.CurrentItem.Location);

            previous = playlist.Previous();
            playlist.Set(previous);
            Assert.AreEqual(previous.Location, playlist.CurrentItem.Location);
            Assert.AreEqual(third.Location, playlist.CurrentItem.Location);

            previous = playlist.Previous();
            playlist.Set(previous);
            Assert.AreEqual(previous.Location, playlist.CurrentItem.Location);
            Assert.AreEqual(second.Location, playlist.CurrentItem.Location);

            previous = playlist.Previous();
            playlist.Set(previous);
            Assert.AreEqual(previous.Location, playlist.CurrentItem.Location);
            Assert.AreEqual(first.Location, playlist.CurrentItem.Location);

            previous = playlist.Previous();
            playlist.Set(previous);
            Assert.AreEqual(null, previous);
            Assert.AreEqual(first.Location, playlist.CurrentItem.Location);
        }

        [TestMethod]
        public void PlaylistRepeatModeNonePreviousTest()
        {
            var service = new TestDataService();
            var playlist = new Playlist(service.GetMediaItems());
            var first = playlist.ElementAt(0);
            var second = playlist.ElementAt(1);
            var third = playlist.ElementAt(2);
            var fourth = playlist.ElementAt(3);

            playlist.RepeatMode = RepeatMode.None;

            Assert.AreEqual(playlist.CurrentItem.Location, first.Location);

            var next = playlist.Next();
            Assert.AreEqual(next.Location, second.Location);
            Assert.AreEqual(first.Location, playlist.CurrentItem.Location);

            playlist.Set(next);
            Assert.AreEqual(next.Location, playlist.CurrentItem.Location);
            Assert.AreEqual(second.Location, playlist.CurrentItem.Location);

            next = playlist.Next();
            playlist.Set(next);
            Assert.AreEqual(next.Location, playlist.CurrentItem.Location);
            Assert.AreEqual(third.Location, playlist.CurrentItem.Location);

            next = playlist.Next();
            playlist.Set(next);
            Assert.AreEqual(next.Location, playlist.CurrentItem.Location);
            Assert.AreEqual(fourth.Location, playlist.CurrentItem.Location);

            var previous = playlist.Previous();
            playlist.Set(previous);
            Assert.AreEqual(previous.Location, playlist.CurrentItem.Location);
            Assert.AreEqual(third.Location, playlist.CurrentItem.Location);

            previous = playlist.Previous();
            playlist.Set(previous);
            Assert.AreEqual(previous.Location, playlist.CurrentItem.Location);
            Assert.AreEqual(second.Location, playlist.CurrentItem.Location);

            previous = playlist.Previous();
            playlist.Set(previous);
            Assert.AreEqual(previous.Location, playlist.CurrentItem.Location);
            Assert.AreEqual(first.Location, playlist.CurrentItem.Location);

            previous = playlist.Previous();
            playlist.Set(previous);
            Assert.AreEqual(null, previous);
            Assert.AreEqual(first.Location, playlist.CurrentItem.Location);
        }
    }
}
