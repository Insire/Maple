using System.Collections.Generic;

namespace InsireBotCore
{
    public class DesignTimeDataService : IDataService
    {
        public IEnumerable<AudioDevice> GetPlaybackDevices()
        {
            yield return new AudioDevice(2, 2, 2, "TestDevice #1", 2, 2, 2, 2);
            yield return new AudioDevice(2, 2, 2, "TestDevice #2", 2, 2, 2, 2);
        }
    }
}
