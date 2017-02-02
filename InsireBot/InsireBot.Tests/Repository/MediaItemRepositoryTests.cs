using Maple.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace Maple.Tests
{
    [TestClass()]
    public class MediaItemRepositoryTests
    {
        private static MediaItemRepository _repository;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            _repository = new MediaItemRepository(new DBConnection(context.DeploymentDirectory));
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
        public void MediaItemRepositoryDeleteByIdTest()
        {
            var item = CreateOne();
            var result = _repository.Delete(item.Id);

            Assert.IsTrue(result == 1);
        }

        [TestMethod()]
        public void MediaItemRepositoryDeleteTest()
        {
            var item = CreateOne();
            var result = _repository.Delete(item);

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
            item.Title = "MediaItemRepositoryUpdateTest";
            _repository.Update(item);

            var readItem = _repository.Read(item.Id);
            Assert.AreEqual("MediaItemRepositoryUpdateTest", readItem.Title);
        }

        [TestMethod()]
        public void MediaItemRepositorySaveUpdateTest()
        {
            var item = CreateOne();
            item.Title = "MediaItemRepositorySaveUpdateTest";

            Assert.IsFalse(item.IsNew);

            item = _repository.Save(item);

            var readItem = _repository.Read(item.Id);
            Assert.AreEqual("MediaItemRepositorySaveUpdateTest", readItem.Title);
        }

        [TestMethod()]
        public void MediaItemRepositorySaveNewTest()
        {
            var item = new Data.MediaItem
            {
                Duration = 2,
                Sequence = 2,
                Title = "MediaItemRepositorySaveNewTest",
            };

            item = _repository.Save(item);

            var readItem = _repository.Read(item.Id);
            Assert.AreEqual("MediaItemRepositorySaveNewTest", readItem.Title);
        }

        [TestMethod()]
        public void MediaItemRepositorySaveDeleteTest()
        {
            var item = new Data.MediaItem
            {
                Duration = 2,
                Sequence = 2,
                Title = "MediaItemRepositorySaveDeleteTest",
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
                Title = "CreateOne",
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
                Title = "CreateMultiple",
            };

            yield return new Data.MediaItem
            {
                Duration = 1,
                Sequence = 1,
                Title = "CreateMultiple_1",
            };

            yield return new Data.MediaItem
            {
                Duration = 1,
                Sequence = 2,
                Title = "CreateMultiple_2",
            };
        }
    }
}