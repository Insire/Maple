using Maple.Core;
using NAudio.Wave;

namespace Maple
{
    public class WavePlayerFactory
    {
        public static IWavePlayer GetPlayer(ILoggingService log)
        {
            var player = default(WaveOutEvent);
            try
            {
                player = new WaveOutEvent
                {
                    DesiredLatency = 200,
                    Volume = 0.5f,
                    DeviceNumber = 0,
                };
            }
            catch (NAudio.MmException ex)
            {
                log.Error(ex);
                // appveyeor does not have any audio devices i guess
            }

            return player;
        }
    }
}
