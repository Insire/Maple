using System;
using System.Diagnostics;
using Maple.Domain;
using NAudio.CoreAudioApi;

namespace Maple
{
    [DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
    internal sealed class WasapiDevice : AudioDevice
    {
        public MMDevice Device { get; }

        public WasapiDevice(MMDevice device)
            : base(device?.FriendlyName)
        {
            Device = device ?? throw new ArgumentNullException(nameof(device));
            Name = device.FriendlyName;

            AudioDeviceTypeId = (int)DeviceType.WASAPI;
        }

        private string GetDebuggerDisplay()
        {
            return $"{this.GetKey()} {Name} {nameof(WasapiDevice)}";
        }
    }
}
