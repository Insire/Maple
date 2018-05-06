using System;

namespace Maple.Domain
{
    [Flags]
    public enum PlaybackState
    {
        None = 0,
        Playing = 1 << 0,
        Paused = 1 << 1,
    }
}
