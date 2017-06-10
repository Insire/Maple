using System;

namespace Maple.Core
{
    public delegate void LoadedEventHandler(object sender, LoadedEventEventArgs e);

    public class LoadedEventEventArgs : EventArgs
    {
    }
}
