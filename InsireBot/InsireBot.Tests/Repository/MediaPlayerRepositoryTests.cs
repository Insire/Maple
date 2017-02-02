using Maple.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace Maple.Tests
{
    [TestClass()]
    public class MediaPlayerRepositoryTests
    {
        private static MediaPlayerRepository _repository;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            _repository = new MediaPlayerRepository(new DBConnection(context.DeploymentDirectory));
        }

        [TestMethod()]
        public void MediaPlayerRepositoryCreateTest()
        {
            var item = CreateOne();
            var item2 = CreateOne();

            Assert.IsTrue(item?.Id != null);
            Assert.IsTrue(item2?.Id != null);
            Assert.IsTrue(item?.Id != item2?.Id);
        }

        [TestMethod()]
        public void MediaPlayerRepositoryDeleteByIdTest()
        {
            var item = CreateOne();
            var result = _repository.Delete(item.Id);

            Assert.IsTrue(result == 1);
        }

        [TestMethod()]
        public void MediaPlayerRepositoryDeleteTest()
        {
            var item = CreateOne();
            var result = _repository.Delete(item);

            Assert.IsTrue(result == 1);
        }

        [TestMethod()]
        public void MediaPlayerRepositoryGetAllTest()
        {
            var items = CreateMultiple().ToList();
            var result = _repository.Create(items);

            Assert.IsTrue(result == 3);
        }

        [TestMethod()]
        public void MediaPlayerRepositoryReadTest()
        {
            var item = CreateOne();
            var readItem = _repository.Read(item.Id);

            Assert.AreEqual(item.DeviceName, readItem.DeviceName);
        }

        [TestMethod()]
        public void MediaPlayerRepositoryUpdateTest()
        {
            var item = CreateOne();
            item.DeviceName = "MediaPlayerRepositoryUpdateTest";
            _repository.Update(item);

            var readItem = _repository.Read(item.Id);
            Assert.AreEqual("MediaPlayerRepositoryUpdateTest", readItem.DeviceName);
        }

        [TestMethod()]
        public void MediaPlayerRepositorySaveUpdateTest()
        {
            var item = CreateOne();
            item.DeviceName = "MediaPlayerRepositorySaveUpdateTest";

            Assert.IsFalse(item.IsNew);

            item = _repository.Save(item);

            var readItem = _repository.Read(item.Id);
            Assert.AreEqual("MediaPlayerRepositorySaveUpdateTest", readItem.DeviceName);
        }

        [TestMethod()]
        public void MediaPlayerRepositorySaveNewTest()
        {
            var item = new Data.MediaPlayer
            {
                DeviceName = "MediaPlayerRepositorySaveNewTest",
                Sequence = 0,
                Name = "MediaPlayerRepositorySaveNewTest",
            };

            item = _repository.Save(item);

            var readItem = _repository.Read(item.Id);
            Assert.AreEqual("MediaPlayerRepositorySaveNewTest", readItem.DeviceName);
        }

        [TestMethod()]
        public void MediaPlayerRepositorySaveDeleteTest()
        {
            var item = new Data.MediaPlayer
            {
                DeviceName = "MediaPlayerRepositorySaveDeleteTest",
                Sequence = 0,
                Name = "MediaPlayerRepositorySaveDeleteTest",
                IsDeleted = true,
            };

            item = _repository.Create(item);
            item = _repository.Save(item);

            var readItem = _repository.Read(item.Id);
            Assert.IsNull(readItem);
        }

        private Data.MediaPlayer CreateOne()
        {
            var item = new Data.MediaPlayer
            {
                DeviceName = "CreateOne",
                Sequence = 0,
                Name = "CreateOne",
            };

            _repository.Create(item);

            return item;
        }

        private IEnumerable<Data.MediaPlayer> CreateMultiple()
        {
            yield return new Data.MediaPlayer
            {
                DeviceName = "CreateMultiple_1",
                Sequence = 0,
                Name = "CreateMultiple_1",
            };

            yield return new Data.MediaPlayer
            {
                DeviceName = "CreateMultiple_2",
                Sequence = 0,
                Name = "CreateMultiple_2",
            };

            yield return new Data.MediaPlayer
            {
                DeviceName = "CreateMultiple_3",
                Sequence = 0,
                Name = "CreateMultiple_3",
            };
        }
    }
}