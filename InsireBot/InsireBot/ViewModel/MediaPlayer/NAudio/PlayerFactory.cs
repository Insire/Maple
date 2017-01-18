using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsireBot.NAudio
{
    public class PlayerFactory
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
