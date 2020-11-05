using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Maple.Domain;
using MvvmScarletToolkit;
using NAudio.CoreAudioApi;
using NAudio.Wave;

namespace Maple
{
    internal sealed class AudioDeviceProvider : IAudioDeviceProvider
    {
        private readonly IScarletDispatcher _dispatcher;

        public AudioDeviceProvider(IScarletDispatcher dispatcher)
        {
            _dispatcher = dispatcher ?? throw new ArgumentNullException(nameof(dispatcher));
        }

        public Task<ReadOnlyCollection<AudioDevice>> Get(Domain.DeviceType type, CancellationToken token)
        {
            return Task.Run(() => (type switch
            {
                Domain.DeviceType.WaveOut => new ReadOnlyCollection<AudioDevice>(GetWave(token).ToList()),
                Domain.DeviceType.DirectSound => new ReadOnlyCollection<AudioDevice>(GetDirectSound(token).ToList()),
                Domain.DeviceType.WASAPI => new ReadOnlyCollection<AudioDevice>(GetWasapi(token).ToList()),
                Domain.DeviceType.ASIO => new ReadOnlyCollection<AudioDevice>(GetAsio(token).ToList()),
                Domain.DeviceType.None => new ReadOnlyCollection<AudioDevice>(Enumerable.Empty<AudioDevice>().ToList()),
                _ => throw new NotImplementedException(),
            }), token);
        }

        private IEnumerable<AudioDevice> GetWave(CancellationToken token)
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

        private IEnumerable<AudioDevice> GetDirectSound(CancellationToken token)
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

        private IEnumerable<AudioDevice> GetWasapi(CancellationToken token)
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

        private IEnumerable<AudioDevice> GetAsio(CancellationToken token)
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

                var device = default(AsioDevice);

                _dispatcher.Invoke(() => device = new AsioDevice(new AsioOut(asio)));

                if (device is null)
                {
                    continue;
                }

                yield return device;
            }
        }
    }
}
