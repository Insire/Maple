using Maple.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Maple
{
    public sealed class MediaPlayerConfiguration : BaseConfiguration<MediaPlayerModel, int>
    {
        public override void Configure(EntityTypeBuilder<MediaPlayerModel> builder)
        {
            base.Configure(builder);

            builder.HasOne(t => t.Playlist)
                .WithMany(t => t.MediaPlayers)
                .IsRequired(false);
        }
    }
}
