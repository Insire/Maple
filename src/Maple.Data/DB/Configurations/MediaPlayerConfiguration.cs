using Maple.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Maple.Data
{
    public sealed class MediaPlayerConfiguration : IEntityTypeConfiguration<MediaPlayerModel>
    {
        public void Configure(EntityTypeBuilder<MediaPlayerModel> builder)
        {
            builder.Property(t => t.Name)
                    .IsRequired()
                    .HasMaxLength(50);

            builder.Property(t => t.DeviceName)
                   .HasMaxLength(100);

            builder.HasKey(t => t.Id);

            builder.Property(t => t.CreatedBy)
                   .HasDefaultValue("SYSTEM");

            builder.Property(t => t.UpdatedBy)
                   .HasDefaultValue("SYSTEM");

            builder.Property(t => t.CreatedOn)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Property(t => t.UpdatedOn)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.HasOne(t => t.Playlist)
                .WithOne(t => t.MediaPlayer)
                .HasForeignKey<MediaPlayerModel>(t => t.PlaylistId)
                .IsRequired(false);
        }
    }
}
