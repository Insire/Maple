using System;

namespace Maple.Core
{
    [Flags]
    public enum VirtualizationViewModelState
    {
        None = 0,
        Expanded = 1 << 0,
        Deflated = 1 << 1,
        Expanding = 1 << 2,
        Deflating = 1 << 3,
    }
}
