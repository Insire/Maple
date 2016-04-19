using System.Collections;
using System.ComponentModel;

namespace InsireBotCore
{
    public interface IPlaylist<IMediaItem> : IIsSelected, IIndex, IList, IRangeCollection<IMediaItem>, INotifyPropertyChanged
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
