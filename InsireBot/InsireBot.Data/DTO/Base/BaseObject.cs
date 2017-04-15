using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Maple.Data
{
    public abstract class BaseObject
    {
        public int Id { get; set; }
        public int Sequence { get; set; }

        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }

        public DateTime UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }

        [NotMapped]
        public bool IsNew => Id == 0;
        [NotMapped]
        public bool IsDeleted { get; set; }
    }
}
