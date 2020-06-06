using System.Diagnostics;

namespace Maple.Domain
{
    [DebuggerDisplay("AudioDeviceModel: {Sequence}, {Name}")]
    public class AudioDeviceModel : Entity<int>
    {
        public string OsId { get; set; }
        public AudioDeviceTypeModel AudioDeviceType { get; }
        public int AudioDeviceTypeId { get; set; }
    }
}
