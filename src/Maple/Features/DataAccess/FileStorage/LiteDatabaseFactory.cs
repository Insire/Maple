using LiteDB;

namespace Maple
{
    public static class LiteDatabaseFactory
    {
        public static LiteDatabase Create(LiteDatabaseOptions options)
        {
            return new LiteDatabase(options.ConnectionString, options.BsonMapper);
        }
    }
}
