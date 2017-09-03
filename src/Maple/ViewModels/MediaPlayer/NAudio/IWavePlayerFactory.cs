using Maple.Interfaces;
using NAudio.Wave;

namespace Maple
{
    public interface IWavePlayerFactory
    {
        IWavePlayer GetPlayer(ILoggingService log);
    }
}