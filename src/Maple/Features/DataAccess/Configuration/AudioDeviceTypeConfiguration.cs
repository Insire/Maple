using Maple.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Maple
{
    public sealed class AudioDeviceTypeConfiguration : BaseConfiguration<AudioDeviceTypeModel, int>
    {
        public override void Configure(EntityTypeBuilder<AudioDeviceTypeModel> builder)
        {
            base.Configure(builder);

            builder.Property(c => c.DeviceType)
                .HasConversion<int>()
                .IsRequired();

            builder.HasData(new AudioDeviceTypeModel
            {
                Id = (int)Domain.DeviceType.WaveOut,
                Sequence = (int)Domain.DeviceType.WaveOut,
                DeviceType = Domain.DeviceType.WaveOut,
                Name = nameof(Domain.DeviceType.WaveOut),
            });

            builder.HasData(new AudioDeviceTypeModel
            {
                Id = (int)Domain.DeviceType.DirectSound,
                Sequence = (int)Domain.DeviceType.DirectSound,
                DeviceType = Domain.DeviceType.DirectSound,
                Name = nameof(Domain.DeviceType.DirectSound),
            });

            builder.HasData(new AudioDeviceTypeModel
            {
                Id = (int)Domain.DeviceType.WASAPI,
                Sequence = (int)Domain.DeviceType.WASAPI,
                DeviceType = Domain.DeviceType.WASAPI,
                Name = nameof(Domain.DeviceType.WASAPI),
            });

            builder.HasData(new AudioDeviceTypeModel
            {
                Id = (int)Domain.DeviceType.ASIO,
                Sequence = (int)Domain.DeviceType.ASIO,
                DeviceType = Domain.DeviceType.ASIO,
                Name = nameof(Domain.DeviceType.ASIO),
            });
        }
    }
}
