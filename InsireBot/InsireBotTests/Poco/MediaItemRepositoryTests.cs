using InsireBot.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace InsireBot.Tests
{
    [TestClass()]
    public class MediaItemRepositoryTests
    {
        private static IMediaItemRepository _repository;

        [ClassInitialize]
        public static void ClassInitialize()
        {
            _repository = new MediaItemRepository();
        }

        [TestMethod()]
        public void CreateTest()
        {
            var item = CreateOne();

            Assert.IsNotNull(item?.Id);
        }

        [TestMethod()]
        public void DeleteTest()
        {
            var item = CreateOne();
            var result = _repository.Delete(item.Id);

            Assert.IsTrue(result == 1);
        }

        [TestMethod()]
        public void GetAllTest()
        {
            var items = CreateMultiple();
            var result = _repository.Create(items);

            Assert.IsTrue(result == 3);
        }

        [TestMethod()]
        public void ReadTest()
        {
            var item = CreateOne();
            var readItem = _repository.Read(item.Id);

            Assert.AreEqual(item.Title, readItem.Title);
        }

        [TestMethod()]
        public void UpdateTest()
        {
            var item = CreateOne();
            item.Title = "Test updated";
            _repository.Update(item);

            var readItem = _repository.Read(item.Id);
            Assert.AreEqual("Test updated", readItem.Title);
        }

        private Data.MediaItem CreateOne()
        {
            var item = new Data.MediaItem
            {
                Duration = 1,
                Sequence = 0,
                Title = "Test",
            };

            _repository.Create(item);

            return item;
        }

        private IEnumerable<Data.MediaItem> CreateMultiple()
        {
            var item = new Data.MediaItem
            {
                Duration = 1,
                Sequence = 0,
                Title = "Test",
            };

            yield return _repository.Create(item);

            var item1 = new Data.MediaItem
            {
                Duration = 1,
                Sequence = 1,
                Title = "Test_1",
            };

            yield return _repository.Create(item1);

            var item2 = new Data.MediaItem
            {
                Duration = 1,
                Sequence = 2,
                Title = "Test_3",
            };

            yield return _repository.Create(item2);
        }
    }
}