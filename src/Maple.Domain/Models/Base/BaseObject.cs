using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Maple.Domain
{
    public abstract class BaseObject : IBaseObject
    {
        [Key]
        [Column(Order = 1)]
        public int Id { get; set; }
        [Column(Order = 2)]
        public int Sequence { get; set; }

        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }

        public DateTime UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        [NotMapped]
        public bool IsDeleted { get; set; }

        [NotMapped]
        public bool IsNew => Id == 0;
    }
}
