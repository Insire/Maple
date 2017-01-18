using NAudio.Wave;
using System.Collections.Generic;

namespace InsireBot
{
    public class PlaybackDeviceFactory
    {
        public static IEnumerable<AudioDevice> GetAudioDevices()
        {
            for (var i = 0; i < WaveOut.DeviceCount; i++)
            {
                var cap = WaveOut.GetCapabilities(1);

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
