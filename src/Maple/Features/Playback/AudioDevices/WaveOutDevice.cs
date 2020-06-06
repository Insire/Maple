using Maple.Domain;
using NAudio.Wave;

namespace Maple
{
    internal sealed class WaveOutDevice : AudioDevice
    {
        public WaveOutCapabilities Device { get; }

        public WaveOutDevice(WaveOutCapabilities device)
            : base(device.ProductName)
        {
            Name = device.ProductName;
            Device = device;

            DeviceType = DeviceType.WaveOut;
        }
    }
}
