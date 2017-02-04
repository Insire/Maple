using Dapper;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using System;

namespace Maple.Data
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
                        + $"({nameof(MediaItem.Id)} INTEGER PRIMARY KEY AUTOINCREMENT, "
                        + $"{nameof(MediaItem.Sequence)} INTEGER, "
                        + $"{nameof(MediaItem.PlaylistId)} INTEGER, "
                        + $"{nameof(MediaItem.PrivacyStatus)} INTEGER, "
                        + $"{nameof(MediaItem.Title)} VARCHAR(255), "
                        + $"{nameof(MediaItem.Location)} VARCHAR(255), "
                        + $"{nameof(MediaItem.Description)} VARCHAR(255), "
                        + $"{nameof(MediaItem.Duration)} INTEGER)";

            using (var connection = SqLiteConnectionFactory.Get(Path))
            {
                connection.Execute(sql);
            }
        }

        public List<MediaItem> GetAll()
        {
            var sql = $"SELECT * FROM {nameof(MediaItem)}";

            using (var connection = SqLiteConnectionFactory.Get(Path))
            {
                return connection.Query<MediaItem>(sql).ToList();
            }
        }

        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public MediaItem Create(MediaItem item)
        {
            var builder = new StringBuilder();
            builder.Append($"INSERT INTO {nameof(MediaItem)} ");
            builder.Append($"({nameof(MediaItem.Title)}, ");
            builder.Append($"{nameof(MediaItem.Sequence)}, ");
            builder.Append($"{nameof(MediaItem.PlaylistId)}, ");
            builder.Append($"{nameof(MediaItem.PrivacyStatus)},");
            builder.Append($"{nameof(MediaItem.Location)}, ");
            builder.Append($"{nameof(MediaItem.Description)}, ");
            builder.Append($"{nameof(MediaItem.Duration)}) ");
            builder.Append($"VALUES(@{nameof(MediaItem.Title)}, ");
            builder.Append($"@{nameof(MediaItem.Sequence)}, ");
            builder.Append($"@{nameof(MediaItem.PlaylistId)}, ");
            builder.Append($"@{nameof(MediaItem.PrivacyStatus)}, ");
            builder.Append($"@{nameof(MediaItem.Location)}, ");
            builder.Append($"@{nameof(MediaItem.Description)}, ");
            builder.Append($"@{nameof(MediaItem.Duration)}); ");
            builder.Append($"SELECT last_insert_rowid();");

            using (var connection = SqLiteConnectionFactory.Get(Path))
            {
                var id = connection.Query<int>(builder.ToString(), item).Single();
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
            var builder = new StringBuilder();
            builder.Append($"INSERT INTO {nameof(MediaItem)} ");
            builder.Append($"({nameof(MediaItem.Title)}, ");
            builder.Append($"{nameof(MediaItem.Sequence)}, ");
            builder.Append($"{nameof(MediaItem.PlaylistId)}, ");
            builder.Append($"{nameof(MediaItem.PrivacyStatus)},");
            builder.Append($"{nameof(MediaItem.Location)}, ");
            builder.Append($"{nameof(MediaItem.Description)}, ");
            builder.Append($"{nameof(MediaItem.Duration)}) ");
            builder.Append($"VALUES(@{nameof(MediaItem.Title)}, ");
            builder.Append($"@{nameof(MediaItem.Sequence)}, ");
            builder.Append($"@{nameof(MediaItem.PlaylistId)}, ");
            builder.Append($"@{nameof(MediaItem.PrivacyStatus)}, ");
            builder.Append($"@{nameof(MediaItem.Location)}, ");
            builder.Append($"@{nameof(MediaItem.Description)}, ");
            builder.Append($"@{nameof(MediaItem.Duration)}); ");
            builder.Append($"SELECT last_insert_rowid();");

            using (var connection = SqLiteConnectionFactory.Get(Path))
            {
                return connection.Execute(builder.ToString(), items);
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
                $"{nameof(MediaItem.PlaylistId)} = @{nameof(MediaItem.PlaylistId)}, " +
                $"{nameof(MediaItem.PrivacyStatus)} = @{nameof(MediaItem.PrivacyStatus)}, " +
                $"{nameof(MediaItem.Location)} = @{nameof(MediaItem.Location)}, " +
                $"{nameof(MediaItem.Description)} = @{nameof(MediaItem.Description)}, " +
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

        public List<MediaItem> GetAllByPlaylistId(int id)
        {
            var sql = $"SELECT * FROM {nameof(MediaItem)}  WHERE {nameof(MediaItem.PlaylistId)} = @Id";

            using (var connection = SqLiteConnectionFactory.Get(Path))
            {
                return connection.Query<MediaItem>(sql, new { Id = id }).ToList();
            }
        }
    }
}
