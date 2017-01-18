using System;
using GalaSoft.MvvmLight;

namespace InsireBot
{
    public class AudioDevice : ObservableObject, IIsSelected, ISequence, IIdentifier
    {
        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                RaisePropertyChanged(nameof(IsSelected));
            }
        }

        /// <summary>
        /// Product name in a null-terminated string 
        /// </summary>
        public string Name { get;  set; }

        /// <summary>
        /// Number specifying whether the device supports mono (1) or stereo (2) output. 
        /// </summary>
        public int Channels { get;  set; }

        public int Sequence { get; set; }

        public Guid ID => Guid.NewGuid();

        /// <summary>
        /// </summary>
        /// <param name="wMid">          
        /// Manufacturer identifier for the device driver for the device. Manufacturer identifiers
        /// are defined in https://msdn.microsoft.com/en-us/library/ms709440.aspx
        /// </param>
        /// <param name="wPid">          
        /// Product identifier for the device. Product identifiers are defined in https://msdn.microsoft.com/en-us/library/ms709440.aspx
        /// </param>
        /// <param name="vDriverVersion">
        /// Version number of the device driver for the device. The high-order byte is the major
        /// version number, and the low-order byte is the minor version number
        /// </param>
        /// <param name="szPname">        Product name in a null-terminated string </param>
        /// <param name="dwFormats">      Standard formats that are supported </param>
        /// <param name="wChannels">     
        /// Number specifying whether the device supports mono (1) or stereo (2) output
        /// </param>
        /// <param name="wReserved">      Packing </param>
        /// <param name="dwSupport">      Optional functionality supported by the device </param>
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
