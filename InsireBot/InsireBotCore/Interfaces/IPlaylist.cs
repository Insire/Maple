using System.Collections;

namespace InsireBotCore
{
    public interface IPlaylist : IIsSelected, IIndex, ICollection
    {
        event RepeatModeChangedEventHandler RepeatModeChanged;
        event ShuffleModeChangedEventHandler ShuffleModeChanged;

        bool CanNext();
        bool CanPrevious();

        IMediaItem Next();
        IMediaItem Previous();

        IMediaItem CurrentItem { get; }

        RepeatMode RepeatMode { get; set; }
        bool IsShuffling { get; set; }
    }
}
