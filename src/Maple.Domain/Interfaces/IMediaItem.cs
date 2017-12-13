using System;

namespace Maple.Domain
{
    public interface IMediaItem : IIsSelected, ISequence, IIdentifier, IChangeState
    {
        string Title { get; }
        string Location { get; }
        TimeSpan Duration { get; }
        PrivacyStatus PrivacyStatus { get; }
    }
}
