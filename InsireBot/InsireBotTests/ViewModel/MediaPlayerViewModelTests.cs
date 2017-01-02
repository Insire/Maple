using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using InsireBotWPF;

namespace InsireBotTests
{
    [TestClass()]
    public class MediaPlayerViewModelTests
    {
        [TestMethod()]
        public void MediaPlayerViewModelTest()
        {
            var service = new TestDataService();
            var vm = new MediaPlayerViewModel(service);

            var playlist = vm.MediaPlayer.Playlist;
            var distinctIndices = playlist.Select(p => p.Index).Distinct().Count();

            Assert.IsTrue(distinctIndices == playlist.Count);
        }

        [TestMethod()]
        public void InitTest()
        {
            var service = new TestDataService();
            var vm = new MediaPlayerViewModel(service);

            var playlist = vm.MediaPlayer.Playlist;
            Assert.IsFalse(vm.IsPlaying);

            Assert.IsNotNull(vm.MediaPlayer);

            Assert.IsNotNull(playlist);
            Assert.IsNotNull(playlist.CurrentItem);

            Assert.IsNotNull(playlist.RepeatMode);

            Assert.IsTrue(playlist.Any());
            Assert.IsTrue(playlist.All(p => p.Index != -1));
        }

        [TestMethod()]
        public void ClearTest()
        {
            var service = new TestDataService();
            var vm = new MediaPlayerViewModel(service);

            var playlist = vm.MediaPlayer.Playlist;

            playlist.Clear();
            Assert.IsFalse(playlist.Any());
        }
    }
}