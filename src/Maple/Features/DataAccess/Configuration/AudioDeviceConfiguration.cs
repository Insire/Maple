using Maple.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Maple
{
    public sealed class AudioDeviceConfiguration : BaseConfiguration<AudioDeviceModel, int>
    {
        public override void Configure(EntityTypeBuilder<AudioDeviceModel> builder)
        {
            base.Configure(builder);

            builder.Property(c => c.OsId)
                .IsRequired();

            builder.HasOne(t => t.AudioDeviceType)
                   .WithMany(t => t.AudioDevices)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
