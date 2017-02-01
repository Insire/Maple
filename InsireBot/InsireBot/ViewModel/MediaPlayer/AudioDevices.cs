using InsireBot.Core;
using System.Linq;

namespace InsireBot
{
    public class AudioDevices : ViewModelListBase<AudioDevice>
    {
        public AudioDevices() : base()
        {
            Items.AddRange(PlaybackDeviceFactory.GetAudioDevices().ToList());
        }
    }
}
