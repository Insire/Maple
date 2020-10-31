using System;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Maple.Domain;
using MvvmScarletToolkit;

namespace Maple
{
    public sealed class AudioDeviceFactory
    {
        private readonly IAudioDeviceProvider _deviceProvider;
        private readonly IScarletCommandBuilder _commandBuilder;
        private readonly ConcurrentDictionary<DeviceType, ReadOnlyCollection<AudioDevice>> _lookup;

        public AudioDeviceFactory(IAudioDeviceProvider deviceProvider, IScarletCommandBuilder commandBuilder)
        {
            _deviceProvider = deviceProvider ?? throw new ArgumentNullException(nameof(deviceProvider));
            _commandBuilder = commandBuilder ?? throw new ArgumentNullException(nameof(commandBuilder));
            _lookup = new ConcurrentDictionary<DeviceType, ReadOnlyCollection<AudioDevice>>();
        }

        public async Task<AudioDevice> Create(AudioDeviceModel model, CancellationToken token)
        {
            if (_lookup.TryGetValue(model.AudioDeviceType.DeviceType, out var cachedDevices))
            {
                return Get(cachedDevices, model);
            }
            else
            {
                var devices = await _deviceProvider.Get(model.AudioDeviceType.DeviceType, token);
                if (_lookup.TryAdd(model.AudioDeviceType.DeviceType, devices))
                {
                    return Get(devices, model);
                }
                else
                {
                    _lookup.TryGetValue(model.AudioDeviceType.DeviceType, out cachedDevices);
                    return Get(cachedDevices, model);
                }
            }
        }

        private static AudioDevice Get(ReadOnlyCollection<AudioDevice> audioDevices, AudioDeviceModel model)
        {
            var result = audioDevices.FirstOrDefault(p => p.OsId == model.OsId);

            result.AssignModel(model);

            return result;
        }

        public CreateAudioDeviceViewModel Create(AudioDevice audioDevice)
        {
            return new CreateAudioDeviceViewModel(_commandBuilder, _deviceProvider, audioDevice);
        }
    }
}