using Maple.Domain;
using NAudio.Wave;
using System.Diagnostics;

namespace Maple
{
    [DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
    internal sealed class WaveOutDevice : AudioDevice
    {
        public WaveOutCapabilities Device { get; }

        public WaveOutDevice(WaveOutCapabilities device)
            : base(device.ProductName)
        {
            Name = device.ProductName;
            Device = device;

            AudioDeviceTypeId = (int)Domain.DeviceType.WaveOut;
        }

        private string GetDebuggerDisplay()
        {
            return $"{this.GetKey()} {Name} {nameof(WaveOutDevice)}";
        }
    }
}
