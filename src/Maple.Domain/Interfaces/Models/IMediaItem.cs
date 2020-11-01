using System;

namespace Maple.Domain
{
    public interface IMediaItem : IEntity<int>
    {
        string Thumbnail { get; }
        string Location { get; }
        TimeSpan Duration { get; }
        PrivacyStatus PrivacyStatus { get; }
        MediaItemType MediaItemType { get; }
    }
}
