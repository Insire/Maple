namespace InsireBot.MediaPlayer
{
    public enum MediaPlayPlaybackType
    {
        Play,           // play everything thats in the playlist once
        RepeatOne,      // play the currently selected MediaItem repeatedly
        RepeatAll,      // play and repeat everything in the playlist
        Random,         // play everything in the playlist in no particular order once
        RandomRepeatAll // play everything in the playlist in no particular order until stopped
    }
}
