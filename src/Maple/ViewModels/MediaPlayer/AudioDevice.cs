using Maple.Core;
using Maple.Domain;

namespace Maple
{
    public class AudioDevice : ObservableObject, IIsSelected, ISequence, IAudioDevice
    {
        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set { SetValue(ref _isSelected, value); }
        }

        private int _channels;
        /// <summary>
        /// Number specifying whether the device supports mono (1) or stereo (2) output.
        /// </summary>
        /// <value>
        /// The channels.
        /// </value>
        public int Channels
        {
            get { return _channels; }
            set { SetValue(ref _channels, value); }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set { SetValue(ref _name, value); }
        }

        private int _sequence;
        public int Sequence
        {
            get { return _sequence; }
            set { SetValue(ref _sequence, value); }
        }

        public AudioDevice(string szPname, short wChannels)
        {
            Name = szPname;
            Channels = wChannels;
        }

        public AudioDevice()
        {
            Name = string.Empty;
            Channels = 0;
        }

        override public string ToString()
        {
            return Name;
        }
    }
}
