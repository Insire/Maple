using Maple.Core;
using System.Linq;

namespace Maple
{
    public class AudioDevices : ViewModelListBase<AudioDevice>
    {
        public AudioDevices() : base()
        {
            Items.AddRange(PlaybackDeviceFactory.GetAudioDevices().ToList());
        }
    }
}
