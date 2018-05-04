using Maple.Domain;

namespace Maple.Core
{
    public class MediaPlayers : ValidableBaseDataListViewModel<MediaPlayer, MediaPlayerModel, int>, IMediaPlayersViewModel
    {
        protected MediaPlayers(ViewModelServiceContainer container)
            : base(container)
        {
        }
    }
}
