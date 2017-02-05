using Dapper;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Transactions;

namespace Maple.Data
{
    public class PlaylistsRepository : IPlaylistsRepository
    {
        private readonly IMediaItemRepository _mediaItemRepository;

        public string Path { get; }

        public PlaylistsRepository(DBConnection connection, IMediaItemRepository mediaItemRepository)
        {
            Path = connection.Path;

            _mediaItemRepository = mediaItemRepository;

            CreateTable();
        }

        public int Create(IEnumerable<Playlist> items)
        {
            var sql = $"INSERT INTO {nameof(Playlist)} "
                + $"({nameof(Playlist.Title)}, {nameof(Playlist.Sequence)}, {nameof(Playlist.RepeatMode)}, {nameof(Playlist.IsShuffeling)}, {nameof(Playlist.Description)}) "
                + $"VALUES(@{nameof(Playlist.Title)}, @{nameof(Playlist.Sequence)}, @{nameof(Playlist.RepeatMode)}, @{nameof(Playlist.IsShuffeling)}, @{nameof(Playlist.Description)}); "
                + "SELECT last_insert_rowid();";

            using (var connection = SqLiteConnectionFactory.Get(Path))
            {
                return connection.Execute(sql, items);
            }
        }

        public Playlist Create(Playlist item)
        {
            var sql = $"INSERT INTO {nameof(Playlist)} "
                + $"({nameof(Playlist.Title)}, {nameof(Playlist.Sequence)}, {nameof(Playlist.RepeatMode)}, {nameof(Playlist.IsShuffeling)}, {nameof(Playlist.Description)}) "
                + $"VALUES(@{nameof(Playlist.Title)}, @{nameof(Playlist.Sequence)}, @{nameof(Playlist.RepeatMode)}, @{nameof(Playlist.IsShuffeling)}, @{nameof(Playlist.Description)}); "
                + "SELECT last_insert_rowid();";

            using (var connection = SqLiteConnectionFactory.Get(Path))
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
                        + $"({nameof(Playlist.Id)} INTEGER PRIMARY KEY AUTOINCREMENT, "
                        + $"{nameof(Playlist.Title)} VARCHAR(255), "
                        + $"{nameof(Playlist.Sequence)} INTEGER, "
                        + $"{nameof(Playlist.RepeatMode)} INTEGER, "
                        + $"{nameof(Playlist.IsShuffeling)} BOOL, "
                        + $"{nameof(Playlist.Description)} VARCHAR(255))";

            using (var connection = SqLiteConnectionFactory.Get(Path))
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
            Debug.WriteLine("Delete PlaylistsRepository");

            var sql = $"DELETE FROM {nameof(Playlist)} WHERE ROWID = @{nameof(Playlist.Id)}";

            using (var connection = SqLiteConnectionFactory.Get(Path))
            {
                return connection.Execute(sql, new { id });
            }
        }

        public List<Playlist> GetAll()
        {
            var sql = $"SELECT * FROM {nameof(Playlist)}";

            using (var connection = SqLiteConnectionFactory.Get(Path))
            {
                var playlists = connection.Query<Playlist>(sql).ToList();
                foreach (var playlist in playlists)
                    playlist.MediaItems = _mediaItemRepository.GetAllByPlaylistId(playlist.Id);

                return playlists;
            }
        }

        public List<Playlist> GetAllById(params int[] ids)
        {
            var sql = $"SELECT * FROM {nameof(Playlist)}  WHERE ROWID = @Ids";

            using (var connection = SqLiteConnectionFactory.Get(Path))
            {
                var playlists = connection.Query<Playlist>(sql, new { Ids = ids }).ToList();
                foreach (var playlist in playlists)
                    playlist.MediaItems = _mediaItemRepository.GetAllByPlaylistId(playlist.Id);

                return playlists;
            }
        }

        public Playlist Read(int id)
        {
            var sql = $"SELECT * FROM {nameof(Playlist)} "
                + $"WHERE ROWID = @{nameof(Playlist.Id)}";

            using (var connection = SqLiteConnectionFactory.Get(Path))
            {
                var playlist = connection
                    .Query<Playlist>(sql, new { Id = id })
                    .SingleOrDefault();

                if (playlist != null)
                    playlist.MediaItems = _mediaItemRepository.GetAllByPlaylistId(playlist.Id);

                return playlist;
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

        public Playlist Update(Playlist playlist)
        {
            Debug.WriteLine("Update PlaylistsRepository");

            var sql = $"UPDATE {nameof(Playlist)} " +
                $"SET {nameof(Playlist.Title)} = @{nameof(Playlist.Title)}, " +
                $"{nameof(Playlist.Sequence)} = @{nameof(Playlist.Sequence)}, " +
                $"{nameof(Playlist.RepeatMode)} = @{nameof(Playlist.RepeatMode)}, " +
                $"{nameof(Playlist.IsShuffeling)} = @{nameof(Playlist.IsShuffeling)}, " +
                $"{nameof(Playlist.Description)} = @{nameof(Playlist.Description)} " +
                $"WHERE ROWID = @{nameof(Playlist.Id)}";

            using (var connection = SqLiteConnectionFactory.Get(Path))
            {
                connection.Execute(sql, playlist);
                return playlist;
            }
        }
    }
}
