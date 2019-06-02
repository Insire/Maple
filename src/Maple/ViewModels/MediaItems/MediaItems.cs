using MvvmScarletToolkit.Abstractions;
using MvvmScarletToolkit.Observables;

namespace Maple
{
    public sealed class MediaItems : ViewModelListBase<MediaItem>
    {
        public MediaItems(ICommandBuilder commandBuilder)
            : base(commandBuilder)
        {
        }
    }
}
