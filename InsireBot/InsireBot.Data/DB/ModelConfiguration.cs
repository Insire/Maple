using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maple.Data
{
    public class ModelConfiguration
    {
        public static void Configure(DbModelBuilder modelBuilder)
        {
            ConfigureCoachEntity(modelBuilder);
            ConfigureMediaItemEntity(modelBuilder);
            ConfigureSelfReferencingEntities(modelBuilder);
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

        private static void ConfigureSelfReferencingEntities(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Foo>();
            //modelBuilder.Entity<FooSelf>();
            //modelBuilder.Entity<FooStep>();
        }
    }
}
