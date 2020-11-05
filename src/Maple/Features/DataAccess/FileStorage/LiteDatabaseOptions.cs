using LiteDB;
using System;

namespace Maple
{
    public sealed class LiteDatabaseOptions
    {
        public string ConnectionString { get; }
        public BsonMapper BsonMapper { get; set; }

        public LiteDatabaseOptions(string connectionString)
        {
            ConnectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }
    }
}
