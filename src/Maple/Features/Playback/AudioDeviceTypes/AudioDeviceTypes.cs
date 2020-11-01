using MvvmScarletToolkit;
using MvvmScarletToolkit.Observables;

namespace Maple
{
    public sealed class AudioDeviceTypes : ViewModelListBase<AudioDeviceType>
    {
        public AudioDeviceTypes(in IScarletCommandBuilder commandBuilder)
            : base(commandBuilder)
        {
        }

        public AudioDeviceType GetById(int id)
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
