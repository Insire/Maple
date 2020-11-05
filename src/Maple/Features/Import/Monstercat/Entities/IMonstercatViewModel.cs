using System;
using System.ComponentModel;

namespace Maple
{
    public interface IMonstercatViewModel : INotifyPropertyChanged
    {
        Guid Id { get; }
        string Title { get; }
    }
}
