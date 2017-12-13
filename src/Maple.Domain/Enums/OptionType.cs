using System;

namespace Maple.Domain
{
    [Flags]
    public enum OptionType
    {
        None = 0,
        Playlist = 1 << 0,
        MediaItem = 1 << 1,
        MediaPlayer = 1 << 2,
        ColorProfile = 1 << 3,
        Scene = 1 << 4,
    }
}
