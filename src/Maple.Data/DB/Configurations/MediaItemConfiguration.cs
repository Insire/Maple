using Maple.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Maple.Data
{
    public sealed class MediaItemConfiguration : IEntityTypeConfiguration<MediaItemModel>
    {
        public void Configure(EntityTypeBuilder<MediaItemModel> builder)
        {
            builder.Property(t => t.Title)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(t => t.Location)
                   .IsRequired()
                   .HasMaxLength(2048);

            builder.HasKey(t => t.Id);

            builder.HasOne(t => t.Playlist)
                   .WithMany(t => t.MediaItems)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Cascade);

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
        }
    }
}
