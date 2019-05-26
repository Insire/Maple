using System;

namespace Maple.Domain
{
    public interface IBaseObject : IChangeState, ISequence
    {
        string CreatedBy { get; set; }
        DateTime CreatedOn { get; set; }

        string UpdatedBy { get; set; }
        DateTime UpdatedOn { get; set; }
    }
}
