using System;

namespace Maple
{
    public delegate void RepeatModeChangedEventHandler(object sender, RepeatModeChangedEventEventArgs e);
    public class RepeatModeChangedEventEventArgs : EventArgs
    {
        public RepeatMode RepeatMode { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RepeatModeChangedEventEventArgs"/> class.
        /// </summary>
        /// <param name="repeatMode">The repeat mode.</param>
        public RepeatModeChangedEventEventArgs(RepeatMode repeatMode)
        {
            RepeatMode = repeatMode;
        }
    }
}
