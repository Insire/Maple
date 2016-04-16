using System.Collections.Generic;

namespace InsireBotCore
{
    public interface IDataService
    {
       IEnumerable<AudioDevice> GetPlaybackDevices();

        IEnumerable<IMediaItem> GetMediaItems();
    }
}
