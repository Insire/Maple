using System;
using System.Diagnostics;
using Maple.Domain;
using NAudio.Wave;

namespace Maple
{
    [DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
    internal sealed class AsioDevice : AudioDevice
    {
        public AsioOut Device { get; }

        public AsioDevice(AsioOut device)
            : base(device?.DriverName)
        {
            Device = device ?? throw new ArgumentNullException(nameof(device));
            Name = device.DriverName;

            AudioDeviceTypeId = (int)Domain.DeviceType.ASIO;
        }

        private string GetDebuggerDisplay()
        {
            return $"{this.GetKey()} {Name} {nameof(AsioDevice)}";
        }
    }
}
