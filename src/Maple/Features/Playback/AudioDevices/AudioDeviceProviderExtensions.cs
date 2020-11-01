using Maple.Domain;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Maple
{
    public static class AudioDeviceProviderExtensions
    {
        public async static Task<List<AudioDevice>> Get(this IAudioDeviceProvider deviceProvider, CancellationToken token)
        {
            var results = new List<AudioDevice>();

            var devices = await deviceProvider.Get(DeviceType.ASIO, token);
            results.AddRange(devices);

            devices = await deviceProvider.Get(DeviceType.DirectSound, token);
            results.AddRange(devices);

            devices = await deviceProvider.Get(DeviceType.WASAPI, token);
            results.AddRange(devices);

            devices = await deviceProvider.Get(DeviceType.WaveOut, token);
            results.AddRange(devices);

            return results;
        }
    }
}
