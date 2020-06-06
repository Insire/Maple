using System.Diagnostics;

namespace Maple.Domain
{
    [DebuggerDisplay("AudioDeviceTypeModel: {Sequence}, {Name}")]
    public class AudioDeviceTypeModel : Entity<int>
    {
        public DeviceType DeviceType { get; set; }
    }
}
