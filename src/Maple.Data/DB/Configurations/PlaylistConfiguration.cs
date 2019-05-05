using Maple.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Maple.Data
{
    public sealed class PlaylistConfiguration : IEntityTypeConfiguration<PlaylistModel>
    {
        public void Configure(EntityTypeBuilder<PlaylistModel> builder)
        {
            builder.Property(t => t.Title)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(t => t.Description)
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

            builder.HasOne(t => t.MediaPlayer)
                .WithOne(t => t.Playlist)
                .HasForeignKey<PlaylistModel>(t => t.MediaPlayerId)
                .IsRequired(false);
        }
    }
}
