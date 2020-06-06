using MvvmScarletToolkit;
using MvvmScarletToolkit.Observables;

namespace Maple
{
    public sealed class AudioDevices : ViewModelListBase<AudioDevice>
    {
        public AudioDevices(IScarletCommandBuilder commandBuilder)
            : base(commandBuilder)
        {
        }
    }
}
