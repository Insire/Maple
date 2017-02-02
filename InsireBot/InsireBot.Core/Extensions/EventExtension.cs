using System;

namespace Maple.Core
{
    public static class EventExtension
    {
        public static void Raise(this EventHandler handler, object thus)
        {
            handler?.Invoke(thus, EventArgs.Empty);
        }
    }
}
