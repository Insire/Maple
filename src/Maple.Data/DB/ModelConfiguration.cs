using System.Data.Entity;

namespace Maple.Data
{
    public static class ModelConfiguration
    {
        public static void Configure(DbModelBuilder modelBuilder)
        {
            ConfigureCoachEntity(modelBuilder);
            ConfigureMediaItemEntity(modelBuilder);
        }

        private static void ConfigureCoachEntity(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MediaPlayer>()
                .HasRequired(p => p.Playlist);
        }

        private static void ConfigureMediaItemEntity(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MediaItem>()
                .HasRequired(p => p.Playlist)
                .WithMany(mediaItem => mediaItem.MediaItems)
                .WillCascadeOnDelete(true);
        }
    }
}
