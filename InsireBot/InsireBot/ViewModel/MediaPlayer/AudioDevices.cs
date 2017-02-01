using InsireBot.Core;

namespace InsireBot
{
    public class AudioDevices : ViewModelListBase<AudioDevice>
    {
        public AudioDevices() : base()
        {
            Items.AddRange(PlaybackDeviceFactory.GetAudioDevices());
        }
    }
}
