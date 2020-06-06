using System;
using NAudio.CoreAudioApi;

namespace Maple
{
    internal sealed class WasapiDevice : AudioDevice
    {
        public MMDevice Device { get; }

        public WasapiDevice(MMDevice device)
            : base(device?.DeviceFriendlyName)
        {
            Device = device ?? throw new ArgumentNullException(nameof(device));
            Name = device.DeviceFriendlyName;
        }
    }
}
