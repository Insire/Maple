using Maple.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Maple
{
    public sealed class PlaylistConfiguration : BaseConfiguration<PlaylistModel, int>
    {
        public override void Configure(EntityTypeBuilder<PlaylistModel> builder)
        {
            base.Configure(builder);

            builder.Property(t => t.IsShuffeling)
                    .IsRequired();

            builder.Property(c => c.PrivacyStatus)
                    .HasConversion<int>()
                    .IsRequired();

            builder.Property(c => c.RepeatMode)
                    .HasConversion<int>()
                    .IsRequired();
        }
    }
}
