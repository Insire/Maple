using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Maple.Domain;
using NAudio.CoreAudioApi;
using NAudio.Wave;

namespace Maple
{
    internal sealed class AudioDeviceProvider : IAudioDeviceProvider
    {
        public Task<ReadOnlyCollection<AudioDevice>> Get(DeviceType type, CancellationToken token)
        {
            return Task.Run(() => type switch
            {
                DeviceType.WaveOut => new ReadOnlyCollection<AudioDevice>(GetWave(token).ToList()),
                DeviceType.DirectSound => new ReadOnlyCollection<AudioDevice>(GetDirectSound(token).ToList()),
                DeviceType.WASAPI => new ReadOnlyCollection<AudioDevice>(GetWasapi(token).ToList()),
                DeviceType.ASIO => new ReadOnlyCollection<AudioDevice>(GetAsio(token).ToList()),
                DeviceType.None => new ReadOnlyCollection<AudioDevice>(Enumerable.Empty<AudioDevice>().ToList()),
                _ => throw new NotImplementedException(),
            }, token);
        }

        private static IEnumerable<AudioDevice> GetWave(CancellationToken token)
        {
            if (token.IsCancellationRequested)
            {
                yield break;
            }

            for (var n = -1; n < WaveOut.DeviceCount; n++)
            {
                if (token.IsCancellationRequested)
                {
                    yield break;
                }

                var caps = WaveOut.GetCapabilities(n);
                yield return new WaveOutDevice(caps);
            }
        }

        private static IEnumerable<AudioDevice> GetDirectSound(CancellationToken token)
        {
            if (token.IsCancellationRequested)
            {
                yield break;
            }

            foreach (var dev in DirectSoundOut.Devices)
            {
                if (token.IsCancellationRequested)
                {
                    yield break;
                }

                yield return new DirectSoundDevice(dev);
            }
        }

        private static IEnumerable<AudioDevice> GetWasapi(CancellationToken token)
        {
            if (token.IsCancellationRequested)
            {
                yield break;
            }

            var enumerator = new MMDeviceEnumerator();
            foreach (var wasapi in enumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active))
            {
                if (token.IsCancellationRequested)
                {
                    yield break;
                }

                yield return new WasapiDevice(wasapi);
            }
        }

        private static IEnumerable<AudioDevice> GetAsio(CancellationToken token)
        {
            if (token.IsCancellationRequested)
            {
                yield break;
            }

            foreach (var asio in AsioOut.GetDriverNames())
            {
                if (token.IsCancellationRequested)
                {
                    yield break;
                }

                yield return new AsioDevice(new AsioOut(asio));
            }
        }
    }
}
