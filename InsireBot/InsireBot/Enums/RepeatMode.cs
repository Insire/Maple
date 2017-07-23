namespace Maple
{
    /// <summary>
    /// defines what happens when the last <see cref="IMediaItem" /> of <see cref="Items" /> is <see cref="SetActive" /> and the <see cref="Next" /> is requested
    /// </summary>
    public enum RepeatMode
    {
        None,           // play everything thats in the playlist once
        Single,         // play the currently selected MediaItem repeatedly
        All,            // play and repeat everything in the playlist
    }
}
