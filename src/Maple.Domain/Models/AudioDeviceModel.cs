using System.Diagnostics;

namespace Maple.Domain
{
    [DebuggerDisplay("AudioDeviceModel: {Sequence}, {Name}")]
    public class AudioDeviceModel : Entity<int>, IAudioDevice
    {
        public string OsId { get; set; }
        public AudioDeviceTypeModel AudioDeviceType { get; set; }
        public int AudioDeviceTypeId { get; set; }
    }
}
