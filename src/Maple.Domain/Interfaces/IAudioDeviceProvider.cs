using System.Collections.Generic;

namespace Maple.Domain
{
    public interface IAudioDeviceProvider
    {
        IEnumerable<IAudioDevice> Get(AudioDeviceType type);
    }
}
