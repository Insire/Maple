using System.Linq;
using Maple.Core;
using Maple.Interfaces;

namespace Maple
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="Maple.Core.BaseListViewModel{Maple.AudioDevice}" />
    public class AudioDevices : BaseListViewModel<IAudioDevice>
    {
        public AudioDevices(ILoggingService log, IMessenger messenger)
            : base(messenger)
        {
            AddRange(PlaybackDeviceFactory.GetAudioDevices(log).ToList());
        }
    }
}
