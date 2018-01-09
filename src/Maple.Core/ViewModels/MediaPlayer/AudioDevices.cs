using System.Linq;
using Maple.Domain;

namespace Maple.Core
{
    public class AudioDevices : BaseListViewModel<IAudioDevice>
    {
        public AudioDevices(ILoggingService log, IMessenger messenger, IPlaybackDeviceFactory factory)
            : base(messenger)
        {
            AddRange(factory.GetAudioDevices(log).ToList());
        }
    }
}
