using Maple.Core;
using NAudio.Wave;
using System;
using System.Collections.Generic;

namespace Maple
{
    public class PlaybackDeviceFactory
    {
        public static IEnumerable<AudioDevice> GetAudioDevices(ILoggingService log)
        {
            for (var i = 0; i < WaveOut.DeviceCount; i++)
            {
                var cap = GetCapabilities(i, log);
                if (Equals(cap, default(WaveOutCapabilities)))
                    break;

                yield return new AudioDevice
                {
                    Channels = cap.Channels,
                    Name = cap.ProductName,
                    Sequence = i,
                };
            }

        }

        private static WaveOutCapabilities GetCapabilities(int index, ILoggingService log)
        {
            try
            {
                return WaveOut.GetCapabilities(index);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return default(WaveOutCapabilities);
            }
        }
    }
}
