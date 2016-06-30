using Microsoft.VisualStudio.TestTools.UnitTesting;
using InsireBotCore;
using System.Linq;
using System.Collections.Generic;
using InsireBot;

namespace InsireBotTests
{
    [TestClass()]
    public class MediaPlayerViewModelTests
    {
        [TestMethod()]
        public void MediaPlayerViewModelTest()
        {
            var service = new RuntimeDataService();
            var vm = new MediaPlayerViewModel(service);

            var playlist = vm.MediaPlayer.Playlist;
            var distinctIndices = playlist.Select(p => p.Index).Distinct().Count();

            Assert.IsTrue(distinctIndices == playlist.Count);
        }

        [TestMethod()]
        public void InitTest()
        {
            var service = new RuntimeDataService();
            var vm = new MediaPlayerViewModel(service);

            var playlist = vm.MediaPlayer.Playlist;
            Assert.IsFalse(vm.IsPlaying);

            Assert.IsNotNull(vm.MediaPlayer);
            Assert.IsNotNull(vm.MediaPlayer.AudioDevice);

            Assert.IsNotNull(playlist);
            Assert.IsNotNull(playlist.CurrentItem);

            Assert.IsNotNull(playlist.RepeatMode);

            Assert.IsTrue(playlist.Any());
            Assert.IsTrue(playlist.All(p => p.Index != -1));
        }

        [TestMethod()]
        public void ClearTest()
        {
            var service = new RuntimeDataService();
            var vm = new MediaPlayerViewModel(service);

            var playlist = vm.MediaPlayer.Playlist;

            playlist.Clear();
            Assert.IsFalse(playlist.Any());
        }

        //[TestMethod()]
        //public void PreviousTest()
        //{
        //    var vm = new MediaPlayerViewModel(new TestDataService());
        //    vm.Previous();
        //    Assert.IsNotNull(vm.Playlist.CurrentItem);
        //}

        //[TestMethod()]
        //public void NextTest()
        //{
        //    var vm = new MediaPlayerViewModel(new TestDataService());

        //    vm.Next();


        //    Assert.IsNotNull(vm.Playlist.CurrentItem);
        //}
    }
}