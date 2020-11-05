using Maple.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Maple
{
    public sealed class MediaItemConfiguration : BaseConfiguration<MediaItemModel, int>
    {
        public override void Configure(EntityTypeBuilder<MediaItemModel> builder)
        {
            base.Configure(builder);

            builder.HasOne(t => t.Playlist)
                   .WithMany(t => t.MediaItems)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
