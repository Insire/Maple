using System;

namespace Maple.Domain
{
    [Flags]
    public enum PlaybackState
    {
        Stopped = 0,
        Playing = 1 << 0,
        Paused = 1 << 1,
    }
}
