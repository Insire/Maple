using Microsoft.VisualStudio.TestTools.UnitTesting;
using InsireBotCore;
using InsireBot.ViewModel;
using System.Linq;

namespace InsireBotTests
{
    [TestClass()]
    public class MediaPlayerViewModelTests
    {
        [TestMethod()]
        public void MediaPlayerViewModelTest()
        {
            var vm = new MediaPlayerViewModel(new RuntimeDataService());

            var playlist = vm.MediaPlayer.Playlist as Playlist;
            Assert.IsFalse(vm.IsPlaying);

            Assert.IsNotNull(vm.MediaPlayer);
            Assert.IsNotNull(vm.MediaPlayer.AudioDevice);

            Assert.IsNotNull(playlist);
            Assert.IsNotNull(playlist.CurrentItem);

            Assert.IsTrue(playlist.Any());
            Assert.IsTrue(playlist.All(p => p.Index != -1));
        }

        [TestMethod()]
        public void PreviousTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void NextTest()
        {
            var vm = new MediaPlayerViewModel(new TestDataService());

            vm.Next();


            //Assert.IsNotNull(vm.NextMediaItem);
        }
    }
}