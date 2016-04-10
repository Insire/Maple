using System.Collections.Generic;

namespace InsireBotCore
{
    public class RuntimeDataService : IDataService
    {
        public IEnumerable<AudioDevice> GetPlaybackDevices()
        {
            var _devices = WinmmService.GetDevCapsPlayback();

            for (int i = 0; i < _devices.Length; i++)
                yield return new AudioDevice(_devices[i].wMid,
                                            _devices[i].wPid,
                                            _devices[i].vDriverVersion,
                                            _devices[i].ToString(),
                                            _devices[i].dwFormats,
                                            _devices[i].wChannels,
                                            _devices[i].wReserved,
                                            _devices[i].dwSupport);
        }
    }
}
