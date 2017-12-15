namespace Maple.Domain
{
    public enum MediaPlayerType
    {
        None = 0,
        FFMPEG = 1 << 0,
        NAUDIO = 1 << 1,
    }
}
