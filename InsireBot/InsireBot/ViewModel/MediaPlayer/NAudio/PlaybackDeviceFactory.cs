using NAudio.Wave;
using System.Collections.Generic;

namespace Maple
{
    public class PlaybackDeviceFactory
    {
        public static IEnumerable<AudioDevice> GetAudioDevices()
        {
            for (var i = 0; i < WaveOut.DeviceCount; i++)
            {
                var cap = WaveOut.GetCapabilities(i);

                yield return new AudioDevice
                {
                    Channels = cap.Channels,
                    Name = cap.ProductName,
                    Sequence = i,
                };
            }
        }
    }
}
