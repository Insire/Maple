﻿using System.ComponentModel;

namespace Maple.Domain
{
    public interface IAudioDevice : INotifyPropertyChanged
    {
        int Channels { get; set; }
        bool IsSelected { get; set; }
        string Name { get; set; }
        int Sequence { get; set; }

        string ToString();
    }
}
