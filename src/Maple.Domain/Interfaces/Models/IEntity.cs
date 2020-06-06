using System;

namespace Maple.Domain
{
    public interface IEntity : ISequence
    {
        string CreatedBy { get; set; }
        DateTime CreatedOn { get; set; }

        string UpdatedBy { get; set; }
        DateTime UpdatedOn { get; set; }

        bool IsNew { get; }

        bool IsDeleted { get; set; }
    }
}
