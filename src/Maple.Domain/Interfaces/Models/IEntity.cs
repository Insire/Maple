using System;

namespace Maple.Domain
{
    public interface IEntity : ISequence
    {
        string Name { get; }

        string CreatedBy { get; }
        DateTime CreatedOn { get; }

        string UpdatedBy { get; }
        DateTime UpdatedOn { get; }

        bool IsDeleted { get; }
    }
}
