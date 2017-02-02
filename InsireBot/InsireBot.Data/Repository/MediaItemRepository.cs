using Dapper;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace InsireBot.Data
{
    public class MediaItemRepository : IMediaItemRepository
    {
        public string Path { get; }
        public MediaItemRepository(DBConnection connection)
        {
            Path = connection.Path;

            CreateTable();
        }

        public void CreateTable()
        {
            var sql = $"CREATE TABLE IF NOT EXISTS "
                    + $"{nameof(MediaItem)} "
                        + $"({nameof(MediaItem.Id)} INT PRIMARY KEY, "
                        + $"{nameof(MediaItem.Title)} VARCHAR(255), "
                        + $"{nameof(MediaItem.Sequence)} INT, "
                        + $"{nameof(MediaItem.Duration)} INT)";

            using (var connection = SqLiteConnectionFactory.Get(Path))
            {
                connection.Execute(sql);
            }
        }

        public IEnumerable<MediaItem> GetAll()
        {
            var sql = $"SELECT * FROM {nameof(MediaItem)}";

            using (var connection = SqLiteConnectionFactory.Get(Path))
            {
                return connection.Query<MediaItem>(sql);
            }
        }

        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public MediaItem Create(MediaItem item)
        {
            var sql = $"INSERT INTO {nameof(MediaItem)} "
                + $"({nameof(MediaItem.Title)}, {nameof(MediaItem.Sequence)}, {nameof(MediaItem.Duration)}) "
                + $"VALUES(@{nameof(MediaItem.Title)}, @{nameof(MediaItem.Sequence)}, @{nameof(MediaItem.Duration)}); "
                + "SELECT last_insert_rowid();";

            using (var connection = SqLiteConnectionFactory.Get(Path))
            {
                var id = connection.Query<int>(sql, item).Single();
                item.Id = id;
                return item;
            }
        }

        /// <summary>
        /// BulkInsert
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public int Create(IEnumerable<MediaItem> items)
        {
            var sql = $"INSERT INTO {nameof(MediaItem)} ({nameof(MediaItem.Title)}, {nameof(MediaItem.Sequence)}, {nameof(MediaItem.Duration)}) "
                + $"VALUES (@{nameof(MediaItem.Title)}, @{nameof(MediaItem.Sequence)}, @{nameof(MediaItem.Duration)}); "
                + "SELECT last_insert_rowid();";

            using (var connection = SqLiteConnectionFactory.Get(Path))
            {
                return connection.Execute(sql, items);
            }
        }

        /// <summary>
        /// Get by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public MediaItem Read(int id)
        {
            var sql = $"SELECT * FROM {nameof(MediaItem)} "
                + $"WHERE ROWID = @{nameof(MediaItem.Id)}";

            using (var connection = SqLiteConnectionFactory.Get(Path))
            {
                return connection
                    .Query<MediaItem>(sql, new { Id = id })
                    .SingleOrDefault();
            }
        }

        public MediaItem Update(MediaItem item)
        {
            var sql = $"UPDATE {nameof(MediaItem)} " +
                $"SET {nameof(MediaItem.Title)} = @{nameof(MediaItem.Title)}, " +
                $"{nameof(MediaItem.Sequence)} = @{nameof(MediaItem.Sequence)}, " +
                $"{nameof(MediaItem.Duration)} = @{nameof(MediaItem.Duration)} " +
                $"WHERE ROWID = @{nameof(MediaItem.Id)}";

            using (var connection = SqLiteConnectionFactory.Get(Path))
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
            var sql = $"DELETE FROM {nameof(MediaItem)} WHERE ROWID = @{nameof(MediaItem.Id)}";

            using (var connection = SqLiteConnectionFactory.Get(Path))
            {
                return connection.Execute(sql, new { id });
            }
        }

        public MediaItem Save(MediaItem item)
        {
            var result = item;
            using (var txScope = new TransactionScope())
            {
                if (item.IsNew)
                    result = Create(item);
                else
                {
                    if (item.IsDeleted)
                        Delete(item.Id);
                    else
                        result = Update(item);
                }

                txScope.Complete();
                return result;
            }
        }

        public List<MediaItem> GetAllById(params int[] ids)
        {
            var sql = $"SELECT * FROM {nameof(MediaItem)}  WHERE ROWID = @Ids";

            using (var connection = SqLiteConnectionFactory.Get(Path))
            {
                return connection.Query<MediaItem>(sql, new { Ids = ids }).ToList();
            }
        }
    }
}
