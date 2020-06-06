using System;
using NAudio.Wave;

namespace Maple
{
    internal sealed class DirectSoundDevice : AudioDevice
    {
        public DirectSoundDeviceInfo Device { get; }

        public DirectSoundDevice(DirectSoundDeviceInfo device)
            : base(device?.ModuleName)
        {
            Device = device ?? throw new ArgumentNullException(nameof(device));
            Name = device.ModuleName;
        }
    }
}
