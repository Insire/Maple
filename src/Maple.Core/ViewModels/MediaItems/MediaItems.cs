using Maple.Domain;

namespace Maple.Core
{
    public class MediaItems : ValidableBaseDataListViewModel<MediaItem, MediaItemModel, int>
    {
        protected MediaItems(ViewModelServiceContainer container)
            : base(container)
        {
        }
    }
}
