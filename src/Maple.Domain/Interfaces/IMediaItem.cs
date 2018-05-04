using System;

namespace Maple.Domain
{
    public interface IMediaItem : IIsSelected, ISequence
    {
        string Title { get; }
        string Location { get; }
        TimeSpan Duration { get; }
        PrivacyStatus PrivacyStatus { get; }
        MediaItemType MediaItemType { get; }
    }
}
