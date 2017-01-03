using System.Collections.Generic;

namespace InsireBot
{
    public interface IDataService
    {
        /// <summary>
        /// returns the audiodevices where the mediaplayers playback their mediaitems
        /// </summary>
        /// <returns></returns>
        IEnumerable<AudioDevice> GetPlaybackDevices();
        /// <summary>
        /// returns all? the mediaitems
        /// </summary>
        /// <returns></returns>
        IEnumerable<IMediaItem> GetMediaItems();
        /// <summary>
        /// returns the mediaitems from the current playlist
        /// </summary>
        /// <returns></returns>
        IEnumerable<IMediaItem> GetCurrentMediaItems();
        /// <summary>
        /// returns an instance of a mediaplayer
        /// </summary>
        /// <returns></returns>
        IMediaPlayer<IMediaItem> GetMediaPlayer();
        /// <summary>
        /// returns a settingsobject for configuration of a mediaplayer
        /// </summary>
        /// <returns></returns>
        ISettings GetMediaPlayerSettings();
        /// <summary>
        /// returns the type of all currently used mediaplayers (all have the same type)
        /// </summary>
        /// <returns></returns>
        MediaPlayerType GetMediaPlayerType();
    }
}
