using System;
using Maple.Domain;
using NAudio.Wave;

namespace Maple
{
    internal sealed class AsioDevice : AudioDevice
    {
        public AsioOut Device { get; }

        public AsioDevice(AsioOut device)
            : base(device?.DriverName)
        {
            Device = device ?? throw new ArgumentNullException(nameof(device));
            Name = device.DriverName;

            DeviceType = DeviceType.ASIO;
        }
    }
}
