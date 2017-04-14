using System.Runtime.InteropServices;

namespace Maple.Core
{
    /// <summary>
    /// Windows Multi Media Service aka recording- and playback devices
    /// </summary>
    public class WinmmService
    {
        /// <summary>
        /// Gets the dev caps playback.
        /// </summary>
        /// <returns></returns>
        public static WAVEOUTCAPS[] GetDevCapsPlayback()
        {
            var waveOutDevicesCount = waveOutGetNumDevs();
            if (waveOutDevicesCount > 0)
            {
                var list = new WAVEOUTCAPS[waveOutDevicesCount];
                for (var uDeviceID = 0; uDeviceID < waveOutDevicesCount; uDeviceID++)
                {
                    var waveOutCaps = new WAVEOUTCAPS();
                    waveOutGetDevCaps(uDeviceID, ref waveOutCaps, Marshal.SizeOf(typeof(WAVEOUTCAPS)));
                    list[uDeviceID] = waveOutCaps;
                }
                return list;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the dev caps recording.
        /// </summary>
        /// <returns></returns>
        public static WAVEINCAPS[] GetDevCapsRecording()
        {
            var waveInDevicesCount = waveInGetNumDevs();
            if (waveInDevicesCount > 0)
            {
                var list = new WAVEINCAPS[waveInDevicesCount];
                for (var uDeviceID = 0; uDeviceID < waveInDevicesCount; uDeviceID++)
                {
                    var waveInCaps = new WAVEINCAPS();
                    waveInGetDevCaps(uDeviceID, ref waveInCaps, Marshal.SizeOf(typeof(WAVEINCAPS)));
                    list[uDeviceID] = waveInCaps;
                }
                return list;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        ///
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 4, CharSet = CharSet.Auto)]
        public struct WAVEOUTCAPS
        {
            public short wMid;
            public short wPid;
            public int vDriverVersion;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            private string szPname;

            public uint dwFormats;
            public short wChannels;
            public short wReserved;
            public uint dwSupport;

            public override string ToString()
            {
                return string.Format("{0}", new object[] { szPname });
            }
        }

        /// <summary>
        ///
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 4, CharSet = CharSet.Auto)]
        public struct WAVEINCAPS
        {
            public short wMid;
            public short wPid;
            public int vDriverVersion;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string szPname;

            public uint dwFormats;
            public short wChannels;
            public short wReserved;
            //public int dwSupport;

            public override string ToString()
            {
                return string.Format("wMid:{0}|wPid:{1}|vDriverVersion:{2}|'szPname:{3}'|dwFormats:{4}|wChannels:{5}|wReserved:{6}", new object[] { wMid, wPid, vDriverVersion, szPname, dwFormats, wChannels, wReserved });
            }
        }


        [DllImport("winmm.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern uint waveInGetDevCaps(int hwo, ref WAVEINCAPS pwic, /*uint*/ int cbwic);

        [DllImport("winmm.dll", SetLastError = true)]
        private static extern uint waveInGetNumDevs();

        [DllImport("winmm.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern uint waveOutGetDevCaps(int hwo, ref WAVEOUTCAPS pwoc, /*uint*/ int cbwoc);

        [DllImport("winmm.dll", SetLastError = true)]
        private static extern uint waveOutGetNumDevs();
    }
}
