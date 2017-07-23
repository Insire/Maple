using Maple.Core;
using NAudio.Wave;

namespace Maple
{
    public interface IWavePlayerFactory
    {
        IWavePlayer GetPlayer(ILoggingService log);
    }
}