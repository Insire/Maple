using System;

namespace InsireBot.MediaPlayer
{
    public interface IMediaItem
    {
        string Title { get; }
        string Location { get; }

        double Duration { get; }

        bool IsRestricted { get; }

        Guid ID { get; }
    }
}
