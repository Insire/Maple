using System.ComponentModel;

namespace Maple.Domain
{
    public interface IAudioDevice : INotifyPropertyChanged, IIsSelected, ISequence
    {
        int Channels { get; set; }
        string Name { get; set; }

        string ToString();
    }
}
