using System;
using System.Windows.Input;

namespace Maple.Core
{
    public interface ISplashScreenViewModel : IDisposable
    {
        ICommand DisposeCommand { get; }
        ICommand LoadCommand { get; }
        string Message { get; }
        string Version { get; }
    }
}