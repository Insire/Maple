using Maple.Domain;
using MvvmScarletToolkit.Observables;

namespace Maple
{
    internal abstract class BaseDevice : ObservableObject, IAudioDevice
    {
        private int channels;
        public int Channels
        {
            get { return channels; }
            protected set { SetValue(ref channels, value); }
        }

        private string name;

        public string Name
        {
            get { return name; }
            protected set { SetValue(ref name, value); }
        }

        private AudioDeviceType type;
        public AudioDeviceType Type
        {
            get { return type; }
            protected set { SetValue(ref type, value); }
        }

        public bool IsSelected { get; set; }
        public int Sequence { get; set; }
    }
}
