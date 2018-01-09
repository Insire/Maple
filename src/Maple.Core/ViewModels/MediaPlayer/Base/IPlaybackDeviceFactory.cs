using System.Collections.Generic;
using Maple.Domain;

namespace Maple.Core
{
    public interface IPlaybackDeviceFactory
    {
        IEnumerable<IAudioDevice> GetAudioDevices(ILoggingService log);
    }
}