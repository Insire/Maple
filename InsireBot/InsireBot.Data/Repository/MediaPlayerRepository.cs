using Dapper;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System;

namespace InsireBot.Data
{
    public class MediaPlayerRepository : IMediaPlayerRepository
    {
        public MediaPlayerRepository()
        {
            CreateTable();
        }

        public int Create(IEnumerable<MediaPlayer> items)
        {
            var sql = $"INSERT INTO {nameof(MediaPlayer)} "
                + $"({nameof(MediaPlayer.PlaylistId)}, {nameof(MediaPlayer.Name)}, {nameof(MediaPlayer.Sequence)}, {nameof(MediaPlayer.DeviceName)}) "
                + $"VALUES(@{nameof(MediaPlayer.PlaylistId)}, @{nameof(MediaPlayer.Name)}, @{nameof(MediaPlayer.Sequence)}, @{nameof(MediaPlayer.DeviceName)}); "
                + "SELECT last_insert_rowid();";

            using (var connection = SqLiteConnectionFactory.Get())
            {
                return connection.Execute(sql, items);
            }
        }

        public MediaPlayer Create(MediaPlayer item)
        {
            var sql = $"INSERT INTO {nameof(MediaPlayer)} "
                + $"({nameof(MediaPlayer.PlaylistId)}, {nameof(MediaPlayer.Name)}, {nameof(MediaPlayer.Sequence)}, {nameof(MediaPlayer.DeviceName)}) "
                + $"VALUES(@{nameof(MediaPlayer.PlaylistId)}, @{nameof(MediaPlayer.Name)}, @{nameof(MediaPlayer.Sequence)}, @{nameof(MediaPlayer.DeviceName)}); "
                + "SELECT last_insert_rowid();";

            using (var connection = SqLiteConnectionFactory.Get())
            {
                var id = connection.Query<int>(sql, item).Single();
                item.Id = id;
                return item;
            }
        }

        public void CreateTable()
        {
            var sql = $"CREATE TABLE IF NOT EXISTS "
                    + $"{nameof(MediaPlayer)} "
                        + $"({nameof(MediaPlayer.Id)} INT PRIMARY KEY, "
                        + $"{nameof(MediaPlayer.PlaylistId)} INT, "
                        + $"{nameof(MediaPlayer.Name)} VARCHAR(255), "
                        + $"{nameof(MediaPlayer.Sequence)} INT, "
                        + $"{nameof(MediaPlayer.DeviceName)} VARCHAR(255)) ";

            using (var connection = SqLiteConnectionFactory.Get())
            {
                connection.Execute(sql);
            }
        }

        public int Delete(MediaPlayer item)
        {
            return Delete(item.Id);
        }

        public int Delete(int id)
        {
            var sql = $"DELETE FROM {nameof(MediaPlayer)} WHERE ROWID = @{nameof(MediaPlayer.Id)}";
            using (var connection = SqLiteConnectionFactory.Get())
            {
                return connection.Execute(sql, new { id });
            }
        }

        public IEnumerable<MediaPlayer> GetAll()
        {
            var sql = $"SELECT * FROM {nameof(MediaPlayer)}";

            using (var connection = SqLiteConnectionFactory.Get())
            {
                return connection.Query<MediaPlayer>(sql);
            }
        }

        public List<MediaPlayer> GetAllById(params int[] ids)
        {
            var sql = $"SELECT * FROM {nameof(MediaPlayer)}  WHERE ROWID = @Ids";

            using (var connection = SqLiteConnectionFactory.Get())
            {
                return connection.Query<MediaPlayer>(sql, new { Ids = ids }).ToList();
            }
        }

        public IEnumerable<MediaPlayer> GetPrimary()
        {
            var sql = $"SELECT * FROM {nameof(MediaPlayer)}  WHERE {nameof(MediaPlayer.IsPrimary)} = True";

            using (var connection = SqLiteConnectionFactory.Get())
            {
                return connection.Query<MediaPlayer>(sql).ToList();
            }
        }

        public MediaPlayer Read(int id)
        {
            var sql = $"SELECT * FROM {nameof(MediaPlayer)} "
                + $"WHERE ROWID = @{nameof(MediaPlayer.Id)}";

            using (var connection = SqLiteConnectionFactory.Get())
            {
                return connection
                    .Query<MediaPlayer>(sql, new { Id = id })
                    .SingleOrDefault();
            }
        }

        public MediaPlayer Save(MediaPlayer item)
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

        public MediaPlayer Update(MediaPlayer item)
        {
            var sql = $"UPDATE {nameof(MediaPlayer)} " +
                $"SET {nameof(MediaPlayer.Name)} = @{nameof(MediaPlayer.Name)}, " +
                $"{nameof(MediaPlayer.Sequence)} = @{nameof(MediaPlayer.Sequence)}, " +
                $"{nameof(MediaPlayer.DeviceName)} = @{nameof(MediaPlayer.DeviceName)} " +
                $"WHERE ROWID = @{nameof(MediaPlayer.Id)}";

            using (var connection = SqLiteConnectionFactory.Get())
            {
                connection.Execute(sql, item);
                return item;
            }
        }
    }
}
