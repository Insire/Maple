using System;

namespace Maple.Domain
{
    public interface IBaseObject
    {
        string CreatedBy { get; set; }
        DateTime CreatedOn { get; set; }
        bool IsDeleted { get; set; }
        bool IsNew { get; }

        int Sequence { get; set; }
        string UpdatedBy { get; set; }
        DateTime UpdatedOn { get; set; }
    }
}
