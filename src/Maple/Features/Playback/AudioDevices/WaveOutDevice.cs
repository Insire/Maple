using Maple.Domain;
using NAudio.Wave;

namespace Maple
{
    internal sealed class WaveOutDevice : BaseDevice
    {
        public WaveOutDevice(WaveOutCapabilities capabilities)
        {
            Channels = capabilities.Channels;
            Name = capabilities.ProductName;

            Type = AudioDeviceType.Waveout;
        }
    }
}
