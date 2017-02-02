using InsireBot.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace InsireBot.Tests
{
    [TestClass()]
    public class PlaylistRepositoryTests
    {
        private static PlaylistsRepository _repository;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            _repository = new PlaylistsRepository(new DBConnection(context.DeploymentDirectory));
        }

        [TestMethod()]
        public void PlaylistRepositoryCreateTest()
        {
            var item = CreateOne();
            var item2 = CreateOne();

            Assert.IsTrue(item?.Id != null);
            Assert.IsTrue(item2?.Id != null);
            Assert.IsTrue(item?.Id != item2?.Id);
        }

        [TestMethod()]
        public void PlaylistRepositoryDeleteByIdTest()
        {
            var item = CreateOne();
            var result = _repository.Delete(item.Id);

            Assert.IsTrue(result == 1);
        }

        [TestMethod()]
        public void PlaylistRepositoryDeleteTest()
        {
            var item = CreateOne();
            var result = _repository.Delete(item);

            Assert.IsTrue(result == 1);
        }

        [TestMethod()]
        public void PlaylistRepositoryGetAllTest()
        {
            var items = CreateMultiple().ToList();
            var result = _repository.Create(items);

            Assert.IsTrue(result == 3);
        }

        [TestMethod()]
        public void PlaylistRepositoryReadTest()
        {
            var item = CreateOne();
            var readItem = _repository.Read(item.Id);

            Assert.AreEqual(item.Title, readItem.Title);
        }

        [TestMethod()]
        public void PlaylistRepositoryUpdateTest()
        {
            var item = CreateOne();
            item.Title = "PlaylistRepositoryUpdateTest";
            _repository.Update(item);

            var readItem = _repository.Read(item.Id);
            Assert.AreEqual("PlaylistRepositoryUpdateTest", readItem.Title);
        }

        [TestMethod()]
        public void PlaylistRepositorySaveUpdateTest()
        {
            var item = CreateOne();
            item.Title = "PlaylistRepositorySaveUpdateTest";

            Assert.IsFalse(item.IsNew);

            item = _repository.Save(item);

            var readItem = _repository.Read(item.Id);
            Assert.AreEqual("PlaylistRepositorySaveUpdateTest", readItem.Title);
        }

        [TestMethod()]
        public void PlaylistRepositorySaveNewTest()
        {
            var item = new Data.Playlist
            {
                RepeatMode = 2,
                Sequence = 2,
                Title = "PlaylistRepositorySaveNewTest",
            };

            item = _repository.Save(item);

            var readItem = _repository.Read(item.Id);
            Assert.AreEqual("PlaylistRepositorySaveNewTest", readItem.Title);
        }

        [TestMethod()]
        public void PlaylistRepositorySaveDeleteTest()
        {
            var item = new Data.Playlist
            {
                RepeatMode = 2,
                Sequence = 2,
                Title = "PlaylistRepositorySaveDeleteTest",
                IsDeleted = true,
            };

            item = _repository.Create(item);
            item = _repository.Save(item);

            var readItem = _repository.Read(item.Id);
            Assert.IsNull(readItem);
        }

        private Data.Playlist CreateOne()
        {
            var item = new Data.Playlist
            {
                RepeatMode = 1,
                Sequence = 0,
                Title = "CreateOne",
            };

            _repository.Create(item);

            return item;
        }

        private IEnumerable<Data.Playlist> CreateMultiple()
        {
            yield return new Data.Playlist
            {
                RepeatMode = 1,
                Sequence = 0,
                Title = "CreateMultiple",
            };

            yield return new Data.Playlist
            {
                RepeatMode = 1,
                Sequence = 1,
                Title = "CreateMultiple_1",
            };

            yield return new Data.Playlist
            {
                RepeatMode = 1,
                Sequence = 2,
                Description = string.Empty,

                Title = "CreateMultiple_2",
            };
        }
    }
}