using Maple.Domain;
using MvvmScarletToolkit;
using MvvmScarletToolkit.Observables;

namespace Maple
{
    public sealed class AudioDevices : ViewModelListBase<AudioDevice>
    {
        public AudioDevices(IScarletCommandBuilder commandBuilder, ISequenceService sequenceService)
            : base(commandBuilder)
        {
        }
    }
}
