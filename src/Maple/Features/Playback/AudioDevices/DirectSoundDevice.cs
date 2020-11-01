using System;
using System.Diagnostics;
using Maple.Domain;
using NAudio.Wave;

namespace Maple
{
    [DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
    internal sealed class DirectSoundDevice : AudioDevice
    {
        public DirectSoundDeviceInfo Device { get; }

        public DirectSoundDevice(DirectSoundDeviceInfo device)
            : base(device?.Description)
        {
            Device = device ?? throw new ArgumentNullException(nameof(device));
            Name = device.Description;

            AudioDeviceTypeId = (int)DeviceType.DirectSound;
        }

        private string GetDebuggerDisplay()
        {
            return $"{this.GetKey()} {Name} {nameof(DirectSoundDevice)}";
        }
    }
}
