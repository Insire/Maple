using System;
using MvvmScarletToolkit.Observables;

namespace Maple
{
    public sealed class OptionsViewModel : ObservableObject
    {
        public Cultures Cultures { get; }
        public AudioDevices AudioDevices { get; }
        public AudioDeviceTypes AudioDeviceTypes { get; }

        public OptionsViewModel(Cultures cultures, AudioDevices audioDevices, AudioDeviceTypes audioDeviceTypes)
        {
            Cultures = cultures ?? throw new ArgumentNullException(nameof(cultures));
            AudioDevices = audioDevices ?? throw new ArgumentNullException(nameof(audioDevices));
            AudioDeviceTypes = audioDeviceTypes ?? throw new ArgumentNullException(nameof(audioDeviceTypes));
        }
    }
}
