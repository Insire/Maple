using System.Collections.Generic;

namespace InsireBot
{
    public interface IDataService
    {
       IEnumerable<AudioDevice> GetPlaybackDevices();
    }
}
