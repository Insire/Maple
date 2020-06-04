using System;
using System.Collections.Generic;
using System.Linq;
using Maple.Domain;
using NAudio.CoreAudioApi;
using NAudio.Wave;

namespace Maple
{
    internal sealed class AudioDeviceProvider : IAudioDeviceProvider
    {
        public IEnumerable<IAudioDevice> Get(AudioDeviceType type)
        {
            return type switch
            {
                AudioDeviceType.Waveout => GetWave(),
                AudioDeviceType.DirectSound => GetDirectSound(),
                AudioDeviceType.WASAPI => GetWasapi(),
                AudioDeviceType.ASIO => GetAsio(),
                AudioDeviceType.None => Enumerable.Empty<IAudioDevice>(),
                _ => throw new NotImplementedException(),
            };
        }

        private static IEnumerable<IAudioDevice> GetWave()
        {
            for (var n = -1; n < WaveOut.DeviceCount; n++)
            {
                var caps = WaveOut.GetCapabilities(n);
                yield return new WaveOutDevice(caps);
            }
        }

        private static IEnumerable<IAudioDevice> GetDirectSound()
        {
            foreach (var dev in DirectSoundOut.Devices)
            {
                yield return new DirectSoundDevice(dev);
            }
        }

        private static IEnumerable<IAudioDevice> GetWasapi()
        {
            var enumerator = new MMDeviceEnumerator();
            foreach (var wasapi in enumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active))
            {
                yield return new WasapiDevice(wasapi);
            }
        }

        private static IEnumerable<IAudioDevice> GetAsio()
        {
            foreach (var asio in AsioOut.GetDriverNames())
            {
                yield return new AsioDevice(new AsioOut(asio));
            }
        }
    }
}
