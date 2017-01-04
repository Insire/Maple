using InsireBot.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace InsireBot.Tests
{
    [TestClass()]
    public class MediaItemRepositoryTests
    {
        private static IMediaItemRepository _repository;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            _repository = new MediaItemRepository();
        }

        [TestMethod()]
        public void MediaItemRepositoryCreateTest()
        {
            var item = CreateOne();
            var item2 = CreateOne();

            Assert.IsTrue(item?.Id != null);
            Assert.IsTrue(item2?.Id != null);
            Assert.IsTrue(item?.Id != item2?.Id);
        }

        [TestMethod()]
        public void MediaItemRepositoryDeleteTest()
        {
            var item = CreateOne();
            var result = _repository.Delete(item.Id);

            Assert.IsTrue(result == 1);
        }

        [TestMethod()]
        public void MediaItemRepositoryGetAllTest()
        {
            var items = CreateMultiple().ToList();
            var result = _repository.Create(items);

            Assert.IsTrue(result == 3);
        }

        [TestMethod()]
        public void MediaItemRepositoryReadTest()
        {
            var item = CreateOne();
            var readItem = _repository.Read(item.Id);

            Assert.AreEqual(item.Title, readItem.Title);
        }

        [TestMethod()]
        public void MediaItemRepositoryUpdateTest()
        {
            var item = CreateOne();
            item.Title = "Test updated";
            _repository.Update(item);

            var readItem = _repository.Read(item.Id);
            Assert.AreEqual("Test updated", readItem.Title);
        }

        [TestMethod()]
        public void MediaItemRepositorySaveUpdateTest()
        {
            var item = CreateOne();
            item.Title = "Test updated";

            Assert.IsFalse(item.IsNew);

            item = _repository.Save(item);

            var readItem = _repository.Read(item.Id);
            Assert.AreEqual("Test updated", readItem.Title);
        }

        [TestMethod()]
        public void MediaItemRepositorySaveNewTest()
        {
            var item = new Data.MediaItem
            {
                Duration = 2,
                Sequence = 2,
                Title = "just created",
            };

            item = _repository.Save(item);

            var readItem = _repository.Read(item.Id);
            Assert.AreEqual("just created", readItem.Title);
        }

        [TestMethod()]
        public void MediaItemRepositorySaveDeletedest()
        {
            var item = new Data.MediaItem
            {
                Duration = 2,
                Sequence = 2,
                Title = "just created",
                IsDeleted = true,
            };

            item = _repository.Create(item);
            item = _repository.Save(item);

            var readItem = _repository.Read(item.Id);
            Assert.IsNull(readItem);
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
            yield return new Data.MediaItem
            {
                Duration = 1,
                Sequence = 0,
                Title = "Test",
            };

            yield return new Data.MediaItem
            {
                Duration = 1,
                Sequence = 1,
                Title = "Test_1",
            };

            yield return new Data.MediaItem
            {
                Duration = 1,
                Sequence = 2,
                Title = "Test_3",
            };
        }
    }
}