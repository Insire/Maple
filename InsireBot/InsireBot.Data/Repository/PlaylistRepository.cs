using Dapper;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace InsireBot.Data
{
    public class PlaylistRepository : IRepository<Playlist>
    {
        public PlaylistRepository()
        {
            CreateTable();
        }

        public int Create(IEnumerable<Playlist> items)
        {
            var sql = $"INSERT INTO {nameof(Playlist)} "
                + $"({nameof(Playlist.Title)}, {nameof(Playlist.Sequence)}, {nameof(Playlist.Location)}, {nameof(Playlist.RepeatMode)}, {nameof(Playlist.IsShuffeling)}, {nameof(Playlist.Description)}) "
                + $"VALUES(@{nameof(Playlist.Title)}, @{nameof(Playlist.Sequence)}, @{nameof(Playlist.Location)}, @{nameof(Playlist.RepeatMode)}, @{nameof(Playlist.IsShuffeling)}, @{nameof(Playlist.Description)}); "
                + "SELECT last_insert_rowid();";

            using (var connection = SqLiteConnectionFactory.Get())
            {
                return connection.Execute(sql, items);
            }
        }

        public Playlist Create(Playlist item)
        {
            var sql = $"INSERT INTO {nameof(Playlist)} "
                + $"({nameof(Playlist.Title)}, {nameof(Playlist.Sequence)}, {nameof(Playlist.Location)}, {nameof(Playlist.RepeatMode)}, {nameof(Playlist.IsShuffeling)}, {nameof(Playlist.Description)}) "
                + $"VALUES(@{nameof(Playlist.Title)}, @{nameof(Playlist.Sequence)}, @{nameof(Playlist.Location)}, @{nameof(Playlist.RepeatMode)}, @{nameof(Playlist.IsShuffeling)}, @{nameof(Playlist.Description)}); "
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
                    + $"{nameof(Playlist)} "
                        + $"({nameof(Playlist.Id)} INT PRIMARY KEY, "
                        + $"{nameof(Playlist.Title)} VARCHAR(255), "
                        + $"{nameof(Playlist.Sequence)} INT, "
                        + $"{nameof(Playlist.Location)} VARCHAR(255), "
                        + $"{nameof(Playlist.RepeatMode)} INT, "
                        + $"{nameof(Playlist.IsShuffeling)} BOOL, "
                        + $"{nameof(Playlist.Description)} VARCHAR(255))";

            using (var connection = SqLiteConnectionFactory.Get())
            {
                connection.Execute(sql);
            }
        }

        public int Delete(Playlist item)
        {
            return Delete(item.Id);
        }

        public int Delete(int id)
        {
            var sql = $"DELETE FROM {nameof(Playlist)} WHERE ROWID = @{nameof(Playlist.Id)}";

            using (var connection = SqLiteConnectionFactory.Get())
            {
                return connection.Execute(sql, new { id });
            }
        }

        public IEnumerable<Playlist> GetAll()
        {
            var sql = $"SELECT * FROM {nameof(Playlist)}";

            using (var connection = SqLiteConnectionFactory.Get())
            {
                return connection.Query<Playlist>(sql);
            }
        }

        public List<Playlist> GetAllById(params int[] ids)
        {
            var sql = $"SELECT * FROM {nameof(Playlist)}  WHERE ROWID = @Ids";

            using (var connection = SqLiteConnectionFactory.Get())
            {
                return connection.Query<Playlist>(sql, new { Ids = ids }).ToList();
            }
        }

        public Playlist Read(int id)
        {
            var sql = $"SELECT * FROM {nameof(Playlist)} "
                + $"WHERE ROWID = @{nameof(Playlist.Id)}";

            using (var connection = SqLiteConnectionFactory.Get())
            {
                return connection
                    .Query<Playlist>(sql, new { Id = id })
                    .SingleOrDefault();
            }
        }

        public Playlist Save(Playlist item)
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

        public Playlist Update(Playlist item)
        {
            var sql = $"UPDATE {nameof(Playlist)} " +
                $"SET {nameof(Playlist.Title)} = @{nameof(Playlist.Title)}, " +
                $"{nameof(Playlist.Sequence)} = @{nameof(Playlist.Sequence)}, " +
                $"{nameof(Playlist.Location)} = @{nameof(Playlist.Location)}, " +
                $"{nameof(Playlist.RepeatMode)} = @{nameof(Playlist.RepeatMode)}, " +
                $"{nameof(Playlist.IsShuffeling)} = @{nameof(Playlist.IsShuffeling)}, " +
                $"{nameof(Playlist.Description)} = @{nameof(Playlist.Description)} " +
                $"WHERE ROWID = @{nameof(Playlist.Id)}";

            using (var connection = SqLiteConnectionFactory.Get())
            {
                connection.Execute(sql, item);
                return item;
            }
        }
    }
}
