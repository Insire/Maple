using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Maple.Data
{
    public sealed class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<PlaylistContext>
    {
        public PlaylistContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                 .Build();

            var builder = new DbContextOptionsBuilder<PlaylistContext>()
                .UseSqlite("Data Source=../maple.db;");

            return new PlaylistContext(builder.Options);
        }
    }
}
