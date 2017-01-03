using Dapper;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace InsireBot.Data
{
    public class MediaItemRepository : IMediaItemRepository
    {
        public MediaItemRepository()
        {
            SqLiteConnectionFactory.Seed<MediaItem>();
        }

        public IEnumerable<MediaItem> GetAll()
        {
            var sql = $"SELECT * FROM {nameof(MediaItem)}";

            using (var connection = SqLiteConnectionFactory.Get())
            {
                return connection.Query<MediaItem>("SELECT * FROM MediaItem");
            }
        }

        public MediaItem Create(MediaItem item)
        {
            var sql = $"INSERT INTO {nameof(MediaItem)} ({nameof(MediaItem.Id)}, {nameof(MediaItem.Title)}, {nameof(MediaItem.Sequence)}, {nameof(MediaItem.Duration)}) " +
                $"VALUES(@{nameof(MediaItem)} @{nameof(MediaItem.Id)}, @{nameof(MediaItem.Title)}, @{nameof(MediaItem.Sequence)}, @{nameof(MediaItem.Duration)}); " +
                "SELECT CAST(SCOPE_IDENTITY() as int)";

            using (var connection = SqLiteConnectionFactory.Get())
            {
                var id = connection.Query<int>(sql, item).Single();
                item.Id = id;
                return item;
            }
        }

        /// <summary>
        /// BulkInsert
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int Create(IEnumerable<MediaItem> item)
        {
            var sql = $"INSERT INTO {nameof(MediaItem)} ({nameof(MediaItem.Id)}, {nameof(MediaItem.Title)}, {nameof(MediaItem.Sequence)}, {nameof(MediaItem.Duration)}) " +
                $"VALUES(@{nameof(MediaItem)} @{nameof(MediaItem.Id)}, @{nameof(MediaItem.Title)}, @{nameof(MediaItem.Sequence)}, @{nameof(MediaItem.Duration)}); " +
                "SELECT CAST(SCOPE_IDENTITY() as int)";

            using (var connection = SqLiteConnectionFactory.Get())
            {
                return connection.Execute(sql, item);
            }
        }

        public MediaItem Read(int id)
        {
            var sql = $"SELECT * FROM {nameof(MediaItem)}  WHERE Id= @Id";

            using (var connection = SqLiteConnectionFactory.Get())
            {
                return connection
                    .Query<MediaItem>(sql, new { Id = id })
                    .SingleOrDefault();
            }
        }

        public MediaItem Update(MediaItem item)
        {
            var sql = $"Update {nameof(MediaItem)} " +
                $"SET {nameof(MediaItem.Id)} = @{nameof(MediaItem.Id)}, " +
                $"{nameof(MediaItem.Title)} = @{nameof(MediaItem.Title)}, " +
                $"{nameof(MediaItem.Sequence)} = @{nameof(MediaItem.Sequence)}, " +
                $"{nameof(MediaItem.Duration)} = @{nameof(MediaItem.Duration)} " +
                "WHERE Id = @Id";

            using (var connection = SqLiteConnectionFactory.Get())
            {
                connection.Execute(sql, item);
                return item;
            }
        }

        public int Delete(MediaItem item)
        {
            return Delete(item.Id);
        }

        public int Delete(int id)
        {
            var sql = $"DELETE FROM {nameof(MediaItem)} WHERE Id = @Id";

            using (var connection = SqLiteConnectionFactory.Get())
            {
                return connection.Execute("DELETE FROM Contacts WHERE Id = @Id", new { id });
            }
        }

        public void Save(MediaItem item)
        {
            using (var txScope = new TransactionScope())
            {
                if (item.IsNew)
                {
                    Create(item);
                }
                else
                {
                    if (item.IsDeleted)
                    {
                        Delete(item.Id);
                    }
                    else
                        Update(item);
                }

                txScope.Complete();
            }
        }

        public List<MediaItem> GetById(params int[] ids)
        {
            var sql = $"SELECT * FROM {nameof(MediaItem)}  WHERE Id= @Ids";

            using (var connection = SqLiteConnectionFactory.Get())
            {
                return connection.Query<MediaItem>(sql, new { Ids = ids }).ToList();
            }
        }
    }
}
