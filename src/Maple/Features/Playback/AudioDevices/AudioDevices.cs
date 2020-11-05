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

        public AudioDevice GetById(int? id)
        {
            for (var i = 0; i < Count; i++)
            {
                if (this[i].Id == id)
                {
                    return this[i];
                }
            }

            return null;
        }
    }
}
