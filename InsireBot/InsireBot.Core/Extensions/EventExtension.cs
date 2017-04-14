using System;

namespace Maple.Core
{
    /// <summary>
    ///
    /// </summary>
    public static class EventExtension
    {
        /// <summary>
        /// Raises the specified thus.
        /// </summary>
        /// <param name="handler">The handler.</param>
        /// <param name="thus">The thus.</param>
        public static void Raise(this EventHandler handler, object thus)
        {
            handler?.Invoke(thus, EventArgs.Empty);
        }
    }
}
