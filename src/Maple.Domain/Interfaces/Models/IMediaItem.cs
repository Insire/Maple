using System;

namespace Maple.Domain
{
    public interface IMediaItem : IEntity<int>
    {
        TimeSpan Duration { get; }
        PrivacyStatus PrivacyStatus { get; }
        MediaItemType MediaItemType { get; }
    }
}
