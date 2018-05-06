using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Maple.Domain
{
    public abstract class Entity<T> : IEntity<T>
    {
        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public T Id { get; set; }

        object IEntity.Id => Id;

        private DateTime? createdDate;
        [DataType(DataType.DateTime)]
        public DateTime CreatedOn
        {
            get { return createdDate ?? DateTime.UtcNow; }
            set { createdDate = value; }
        }

        [DataType(DataType.DateTime)]
        public DateTime? UpdatedOn { get; set; }

        public string CreatedBy { get; set; }

        public string UpdatedBy { get; set; }

        [Column(Order = 2)]
        public int Sequence { get; set; }

        [Timestamp]
        public byte[] Version { get; set; }

        [NotMapped]
        public bool IsNew => EqualityComparer<T>.Default.Equals(Id, default);

        [NotMapped]
        public bool IsDeleted { get; set; }
    }
}
