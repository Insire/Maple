using System;

namespace Maple.Domain
{
    [Flags]
    public enum AudioFormats
    {
        None = 0,
        WAV = 1 << 0,
        AAC = 1 << 1,
        WMA = 1 << 2,
        MP3 = 1 << 3,
    }
}
