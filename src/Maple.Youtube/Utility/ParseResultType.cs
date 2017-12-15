using System;

namespace Maple.Youtube
{
    [Flags]
    public enum ParseResultType
    {
        None = 0,
        Playlists = 1 << 0,
        MediaItems = 1 << 1,
    }
}
