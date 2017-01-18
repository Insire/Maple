using NAudio.Wave;

namespace InsireBot
{
    public class WavePlayerFactory
    {
        public static IWavePlayer GetPlayer()
        {
            return new WaveOutEvent
            {
                DesiredLatency = 200,
                Volume = 0.5f,
                DeviceNumber = 0,
            };
        }
    }
}
