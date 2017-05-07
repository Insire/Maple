using System;
using System.Windows.Input;

namespace Maple
{
    public interface ISplashScreenViewModel : IDisposable
    {
        ICommand DisposeCommand { get; }
        ICommand LoadCommand { get; }
        string Message { get; }
        string Version { get; }
    }
}