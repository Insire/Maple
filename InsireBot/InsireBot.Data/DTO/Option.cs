using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace Maple.Data
{
    [DebuggerDisplay("{Key}, {Value}, {Type}")]
    public class Option : BaseObject
    {
        public string Value { get; set; }
        public int Type { get; set; }


        [Required]
        public string Key { get; set; }
    }
}
