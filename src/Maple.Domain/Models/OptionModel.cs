using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace Maple.Domain
{
    [DebuggerDisplay("{Key}, {Value}, {Type}")]
    public class OptionModel : BaseObject
    {
        public string Value { get; set; }
        public int Type { get; set; }


        [Required]
        public string Key { get; set; }
    }
}
