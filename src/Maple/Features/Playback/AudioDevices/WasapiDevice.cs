using NAudio.CoreAudioApi;

namespace Maple
{
    internal sealed class WasapiDevice : BaseDevice
    {
        public WasapiDevice(MMDevice device)
        {
            Name = device.DeviceFriendlyName;
        }
    }
}
