using GalaSoft.MvvmLight;

namespace InsireBot
{
    public class AudioDevice : ObservableObject
    {
        /// <summary>
        /// Product name in a null-terminated string 
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// Version number of the device driver for the device. The high-order byte is the major
        /// version number, and the low-order byte is the minor version number
        /// </summary>
        public int DriverVersion { get; private set; }

        /// <summary>
        /// Standard formats that are supported. Can be a combination of the following:
        /// WAVE_FORMAT_1M08 11.025 kHz, mono, 8-bit WAVE_FORMAT_1M16 11.025 kHz, mono, 16-bit
        /// WAVE_FORMAT_1S08 11.025 kHz, stereo, 8-bit WAVE_FORMAT_1S16 11.025 kHz, stereo, 16-bit
        /// WAVE_FORMAT_2M08 22.05 kHz, mono, 8-bit WAVE_FORMAT_2M16 22.05 kHz, mono, 16-bit
        /// WAVE_FORMAT_2S08 22.05 kHz, stereo, 8-bit WAVE_FORMAT_2S16 22.05 kHz, stereo, 16-bit
        /// WAVE_FORMAT_4M08 44.1 kHz, mono, 8-bit WAVE_FORMAT_4M16 44.1 kHz, mono, 16-bit
        /// WAVE_FORMAT_4S08 44.1 kHz, stereo, 8-bit WAVE_FORMAT_4S16 44.1 kHz, stereo, 16-bit
        /// WAVE_FORMAT_48M08 48 kHz, mono, 8-bit WAVE_FORMAT_48S08 48 kHz, stereo, 8-bi
        /// WAVE_FORMAT_48M16 48 kHz, mono, 16-bit WAVE_FORMAT_48S16 48 kHz, stereo, 16-bit
        /// WAVE_FORMAT_96M08 96 kHz, mono, 8-bit WAVE_FORMAT_96S08 96 kHz, stereo, 8-bit
        /// WAVE_FORMAT_96M16 96 kHz, mono, 16-bit WAVE_FORMAT_96S16 96 kHz, stereo, 16-bit
        /// </summary>
        public uint Formats { get; private set; }

        /// <summary>
        /// Number specifying whether the device supports mono (1) or stereo (2) output. 
        /// </summary>
        public short Channels { get; private set; }

        /// <summary>
        /// Optional functionality supported by the device. The following values are defined:
        /// WAVECAPS_LRVOLUME Supports separate left and right volume control. WAVECAPS_PITCH
        /// Supports pitch control. WAVECAPS_PLAYBACKRATE Supports playback rate control.
        /// WAVECAPS_SYNC The driver is synchronous and will block while playing a buffer.
        /// WAVECAPS_VOLUME Supports volume control. WAVECAPS_SAMPLEACCURATE Returns sample-accurate
        /// position information.
        /// </summary>
        public uint Support { get; private set; }

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
        public AudioDevice(short wMid, short wPid, int vDriverVersion, string szPname, uint dwFormats, short wChannels, short wReserved, uint dwSupport)
        {
            Name = szPname;
            DriverVersion = vDriverVersion;
            Formats = dwFormats;
            Channels = wChannels;
            Support = dwSupport;
        }

        public AudioDevice()
        {
            Name = string.Empty;
            DriverVersion = 0;
            Formats = 0;
            Channels = 0;
            Support = 0;
        }

        override public string ToString()
        {
            return Name;
        }

        public object Value { get; private set; }

    }
}
