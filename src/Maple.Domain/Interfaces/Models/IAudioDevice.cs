using System.ComponentModel;

namespace Maple.Domain
{
    public interface IAudioDevice : INotifyPropertyChanged, IIsSelected, ISequence
    {
        string Name { get; }
        AudioDeviceType Type { get; }
    }
}
