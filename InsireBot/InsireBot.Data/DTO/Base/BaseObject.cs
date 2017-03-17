using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Maple.Data
{
    public abstract class BaseObject
    {
        [Key]
        public int Id { get; set; }
        public int Sequence { get; set; }

        [NotMapped]
        public bool IsNew => Id == 0;
        [NotMapped]
        public bool IsDeleted { get; set; }
    }
}
