using System.Collections;
using System.ComponentModel;

namespace InsireBotCore
{
    public interface IPlaylist<T> : IList, IIsSelected, IIndex, IIdentifier, IRangeCollection<T>, INotifyPropertyChanged where T: IMediaItem
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
