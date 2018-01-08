using System.Collections.Generic;
using Maple.Domain;

namespace Maple
{
    public interface IPlaybackDeviceFactory
    {
        IEnumerable<IAudioDevice> GetAudioDevices(ILoggingService log);
    }
}