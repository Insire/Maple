using System;
using System.Windows.Media;

namespace Maple.Core
{
    public delegate void UiPrimaryColorEventHandler(object sender, UiPrimaryColorEventArgs e);

    public class UiPrimaryColorEventArgs : EventArgs
    {
        public Color Color { get; private set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="UiPrimaryColorEventArgs"/> class.
        /// </summary>
        /// <param name="item">The item.</param>
        public UiPrimaryColorEventArgs(Color item)
        {
            Color = item;
        }
    }
}
