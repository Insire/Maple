using System.Linq;
using Maple.Domain;
using MvvmScarletToolkit.Abstractions;

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
