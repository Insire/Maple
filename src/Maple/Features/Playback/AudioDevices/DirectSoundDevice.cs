using NAudio.Wave;

namespace Maple
{
    internal sealed class DirectSoundDevice : BaseDevice
    {
        public DirectSoundDevice(DirectSoundDeviceInfo directSoundDeviceInfo)
        {
            Name = directSoundDeviceInfo.ModuleName;
        }
    }
}
