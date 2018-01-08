using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Maple.Domain
{
    public abstract class BaseModel<TKeyDataType> : IBaseModel<TKeyDataType>
    {
        [Key]
        [Column(Order = 1)]
        public TKeyDataType Id { get; set; }
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
        public bool IsNew => EqualityComparer<TKeyDataType>.Default.Equals(Id, default(TKeyDataType));
    }
}
