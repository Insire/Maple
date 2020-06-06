using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Maple.Domain;
using MvvmScarletToolkit;
using MvvmScarletToolkit.Observables;

namespace Maple
{
    public sealed class CreateAudioDeviceViewModel : ViewModelListBase<AudioDevice>
    {
        private readonly IAudioDeviceProvider _deviceProvider;

        private DeviceType _deviceType;
        public DeviceType DeviceType
        {
            get { return _deviceType; }
            set { SetValue(ref _deviceType, value); }
        }

        public AudioDevice AudioDevice { get; }

        public ICommand LoadCommand { get; }

        public CreateAudioDeviceViewModel(IScarletCommandBuilder commandBuilder, IAudioDeviceProvider deviceProvider, AudioDevice audioDevice)
            : base(commandBuilder)
        {
            _deviceProvider = deviceProvider ?? throw new ArgumentNullException(nameof(deviceProvider));

            AudioDevice = audioDevice ?? throw new ArgumentNullException(nameof(audioDevice));

            LoadCommand = CommandBuilder
                .Create(Load, CanLoad)
                .WithBusyNotification(BusyStack)
                .WithSingleExecution()
                .Build();
        }

        private async Task Load(CancellationToken token)
        {
            var asio = _deviceProvider.Get(DeviceType.ASIO, token);
            var directSound = _deviceProvider.Get(DeviceType.DirectSound, token);
            var wasapi = _deviceProvider.Get(DeviceType.WASAPI, token);
            var waveout = _deviceProvider.Get(DeviceType.WaveOut, token);

            await Task.WhenAll(asio, directSound, wasapi, waveout);

            await AddRange(asio.Result);
            await AddRange(directSound.Result);
            await AddRange(wasapi.Result);
            await AddRange(waveout.Result);
        }

        private bool CanLoad()
        {
            return true;
        }
    }
}
