using System.Linq;

using Maple.Core;
using Maple.Domain;

namespace Maple
{
    public class AudioDevices : BaseListViewModel<IAudioDevice>
    {
        public AudioDevices(ILoggingService log, IMessenger messenger)
            : base(messenger)
        {
            AddRange(PlaybackDeviceFactory.GetAudioDevices(log).ToList());
        }
    }
}
