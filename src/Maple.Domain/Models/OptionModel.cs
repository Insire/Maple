using System.Diagnostics;

namespace Maple.Domain
{
    [DebuggerDisplay("{Key}, {Value}, {Type}")]
    public class OptionModel : Entity<int>
    {
        public string Value { get; set; }
        public int Type { get; set; }

        public string Key { get; set; }
    }
}
