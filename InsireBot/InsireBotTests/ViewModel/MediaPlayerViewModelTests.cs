using Microsoft.VisualStudio.TestTools.UnitTesting;
using InsireBotCore;
using InsireBotTests;

namespace InsireBot.ViewModel.Tests
{
    [TestClass()]
    public class MediaPlayerViewModelTests
    {
        [TestMethod()]
        public void MediaPlayerViewModelTest()
        {
            var vm = new MediaPlayerViewModel(new TestDataService());

            Assert.IsFalse(vm.IsPlaying);

            Assert.IsNotNull(vm.MediaPlayer);
            Assert.IsNotNull(vm.MediaPlayer.AudioDevice);
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