using Maple.Core;

namespace Maple
{
    public class AudioDevice : ObservableObject, IIsSelected, ISequence
    {
        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set { SetValue(ref _isSelected, value); }
        }

        public string Name { get;  set; }

        /// <summary>
        /// Number specifying whether the device supports mono (1) or stereo (2) output. 
        /// </summary>
        public int Channels { get;  set; }

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
