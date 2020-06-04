using Maple.Domain;
using MvvmScarletToolkit.Observables;

namespace Maple
{
    public sealed class Playlists : ViewModelListBase<Playlist>
    {
        public Playlists(IMapleCommandBuilder commandBuilder)
            : base(commandBuilder)
        {
        }
    }
}
