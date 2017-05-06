using Maple.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Maple.Test.IO
{
    [TestClass]
    public class FileSystemViewModelTests
    {
        private MockRepository _mockRepository;



        [TestInitialize]
        public void TestInitialize()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);


        }

        [TestCleanup]
        public void TestCleanup()
        {
            _mockRepository.VerifyAll();
        }

        [TestMethod]
        public void ViewModelCtorTest()
        {
            var viewModel = CreateViewModel();
        }

        private FileSystemViewModel CreateViewModel()
        {
            return new FileSystemViewModel();
        }
    }
}