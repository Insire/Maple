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
                Id = (int)DeviceType.WaveOut,
                Sequence = (int)DeviceType.WaveOut,
                DeviceType = DeviceType.WaveOut,
                Name = nameof(DeviceType.WaveOut),
            });

            builder.HasData(new AudioDeviceTypeModel
            {
                Id = (int)DeviceType.DirectSound,
                Sequence = (int)DeviceType.DirectSound,
                DeviceType = DeviceType.DirectSound,
                Name = nameof(DeviceType.DirectSound),
            });

            builder.HasData(new AudioDeviceTypeModel
            {
                Id = (int)DeviceType.WASAPI,
                Sequence = (int)DeviceType.WASAPI,
                DeviceType = DeviceType.WASAPI,
                Name = nameof(DeviceType.WASAPI),
            });

            builder.HasData(new AudioDeviceTypeModel
            {
                Id = (int)DeviceType.ASIO,
                Sequence = (int)DeviceType.ASIO,
                DeviceType = DeviceType.ASIO,
                Name = nameof(DeviceType.ASIO),
            });
        }
    }
}
