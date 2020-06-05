using System;

namespace Maple.Domain
{
    public interface IMediaItem : IIsSelected, ISequence
    {
        string Name { get; }
        string Location { get; }
        TimeSpan Duration { get; }
        PrivacyStatus PrivacyStatus { get; }
        MediaItemType MediaItemType { get; }
    }
}
