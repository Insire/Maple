using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Maple.Domain
{
    public abstract class BaseObject<TKey> : IEntity<TKey>
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public TKey Id { get; set; }

        public int Sequence { get; set; }

        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }

        [NotMapped]
        public bool IsDeleted { get; set; }

        [NotMapped]
        public bool IsNew => Id.Equals(default(TKey));
    }
}
