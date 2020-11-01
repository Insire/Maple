using System.Collections.Generic;
using System.Diagnostics;

namespace Maple.Domain
{
    [DebuggerDisplay("AudioDeviceTypeModel: {Sequence}, {Name}")]
    public class AudioDeviceTypeModel : Entity<int>, IAudioDeviceTypeModel
    {
        public DeviceType DeviceType { get; set; }

        public virtual List<AudioDeviceModel> AudioDevices { get; set; }

        public AudioDeviceTypeModel()
        {
            AudioDevices = new List<AudioDeviceModel>();
        }
    }
}
