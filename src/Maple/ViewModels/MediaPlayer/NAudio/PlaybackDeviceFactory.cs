﻿using System;
using System.Collections.Generic;
using Maple.Core;
using Maple.Domain;
using NAudio.Wave;

namespace Maple
{
    public class PlaybackDeviceFactory : IPlaybackDeviceFactory
    {
        public IEnumerable<IAudioDevice> GetAudioDevices(ILoggingService log)
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

        private WaveOutCapabilities GetCapabilities(int index, ILoggingService log)
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
