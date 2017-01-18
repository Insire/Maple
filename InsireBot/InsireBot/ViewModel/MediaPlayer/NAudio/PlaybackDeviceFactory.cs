using System.Collections.Generic;
using Playback = NAudio;

namespace InsireBot
{
    public class PlaybackDeviceFactory
    {
        public static IEnumerable<AudioDevice> GetAudioDevices()
        {
            for (var i = 0; i < Playback.Wave.WaveOut.DeviceCount; i++)
            {
                var cap = Playback.Wave.WaveOut.GetCapabilities(1);

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
