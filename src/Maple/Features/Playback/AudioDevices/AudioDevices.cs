using Maple.Domain;
using MvvmScarletToolkit;
using MvvmScarletToolkit.Observables;

namespace Maple
{
    public sealed class AudioDevices : ViewModelListBase<IAudioDevice>
    {
        public AudioDevices(IScarletCommandBuilder commandBuilder)
            : base(commandBuilder)
        {
        }
    }
}
