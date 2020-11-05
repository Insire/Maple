using LiteDB;
using System;
using System.IO;
using System.Linq;
using System.Windows;

namespace Maple
{
    // https://www.litedb.org/docs/filestorage/
    // https://www.litedb.org/docs/getting-started/
    public sealed class LiteThumbnailCache : IThumbnailCache
    {
        private readonly Func<LiteDatabase> _databaseFactory;
        private readonly string StorageName;

        public LiteThumbnailCache(Func<LiteDatabase> databaseFactory)
        {
            _databaseFactory = databaseFactory ?? throw new ArgumentNullException(nameof(databaseFactory));
            StorageName = "ThumbnailCache";
        }

        public void Add<T>(Stream stream, int id, Size size)
        {
            var type = nameof(T);
            using (var db = _databaseFactory())
            {
                // Gets a FileStorage with custom collection name
                var fs = db.GetStorage<string>(StorageName);

                // Upload a file from a Stream
                fs.Upload(GetCacheKey(type, id, size), ToString(size), stream);
            }
        }

        private static string GetCacheKey(string type, int id, Size? size)
        {
            if (size.HasValue)
            {
                return $"$/{type}/{id}/{ToString(size.Value)}";
            }
            else
            {
                return $"$/{type}/{id}/";
            }
        }

        private static string ToString(Size size)
        {
            return $"{size.Width}x{size.Height}";
        }

        public Stream Get<T>(int id, Size? size)
        {
            var type = nameof(T);
            using (var db = _databaseFactory())
            {
                // Gets a FileStorage with custom collection name
                var fs = db.GetStorage<string>(StorageName);
                var file = fs.Find(GetCacheKey(type, id, size)).FirstOrDefault();

                return file.OpenRead();
            }
        }
    }
}
