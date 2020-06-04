namespace Maple.Domain
{
    public enum RepeatMode
    {
        None = 0,               // play everything thats in the playlist once
        Single = 1 << 0,        // play the currently selected MediaItem repeatedly
        All = 1 << 1,           // play and repeat everything in the playlist
    }
}
