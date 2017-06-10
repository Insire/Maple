using System;

namespace Maple
{
    public delegate void LoadedEventHandler(object sender, LoadedEventEventArgs e);

    public class LoadedEventEventArgs : EventArgs
    {
    }
}
