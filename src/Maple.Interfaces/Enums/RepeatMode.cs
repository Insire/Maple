namespace Maple.Interfaces
{
    /// <summary>
    /// defines what happens when the last <see cref="IMediaItem" /> of <see cref="Items" /> is <see cref="SetActive" /> and the <see cref="Next" /> is requested
    /// </summary>
    public enum RepeatMode
    {
        None = 0,           // play everything thats in the playlist once
        Single = 1 << 0,         // play the currently selected MediaItem repeatedly
        All = 1 << 1,            // play and repeat everything in the playlist
    }
}
