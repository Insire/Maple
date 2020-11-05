using System;

namespace Maple.Domain
{
    public abstract class Entity<TKey> : IEntity<TKey>
    {
        public TKey Id { get; set; }

        public int Sequence { get; set; }

        public string Name { get; set; }

        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }

        public bool IsDeleted { get; set; }
    }
}
