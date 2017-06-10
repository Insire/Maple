using System;

namespace Maple.Core
{
    public delegate void ShuffleModeChangedEventHandler(object sender, ShuffleModeChangedEventEventArgs e);

    public class ShuffleModeChangedEventEventArgs : EventArgs
    {
        public bool IsShuffeling { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShuffleModeChangedEventEventArgs"/> class.
        /// </summary>
        /// <param name="isShuffeling">if set to <c>true</c> [is shuffeling].</param>
        public ShuffleModeChangedEventEventArgs(bool isShuffeling)
        {
            IsShuffeling = isShuffeling;
        }
    }
}
