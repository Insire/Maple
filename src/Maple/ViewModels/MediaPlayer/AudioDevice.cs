using Maple.Core;
using Maple.Interfaces;

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

        /// <summary>
        /// Number specifying whether the device supports mono (1) or stereo (2) output.
        /// </summary>
        /// <value>
        /// The channels.
        /// </value>
        public int Channels { get; set; }
        public string Name { get; set; }
        public int Sequence { get; set; }

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
