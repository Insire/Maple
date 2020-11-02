using LiteDB;
using System;
using System.IO;

namespace Maple
{
    public sealed class LiteMediaItemCache : IMediaItemCache
    {
        private readonly Func<LiteDatabase> _databaseFactory;
        private readonly string StorageName;

        public LiteMediaItemCache(Func<LiteDatabase> databaseFactory)
        {
            _databaseFactory = databaseFactory ?? throw new ArgumentNullException(nameof(databaseFactory));
            StorageName = "MediaItemCache";
        }

        public void Add<T>(Stream stream, int id)
        {
            var type = nameof(T);
            using (var db = _databaseFactory())
            {
                // Gets a FileStorage with custom collection name
                var fs = db.GetStorage<string>(StorageName);

                // Upload a file from a Stream
                fs.Upload(GetCacheKey(type, id), id.ToString(), stream);
            }
        }

        private static string GetCacheKey(string type, int id)
        {
            return $"$/{type}/{id}";
        }

        public Stream Get<T>(int id)
        {
            var type = nameof(T);
            using (var db = _databaseFactory())
            {
                // Gets a FileStorage with custom collection name
                var fs = db.GetStorage<string>(StorageName);
                var file = fs.FindById(GetCacheKey(type, id));

                return file.OpenRead();
            }
        }
    }
}
