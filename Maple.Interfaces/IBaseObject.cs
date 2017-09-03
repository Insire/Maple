using System;

namespace Maple.Interfaces
{
    public interface IBaseObject
    {
        string CreatedBy { get; set; }
        DateTime CreatedOn { get; set; }
        int Id { get; set; }
        bool IsDeleted { get; set; }
        bool IsNew { get; }
        byte[] RowVersion { get; set; }
        int Sequence { get; set; }
        string UpdatedBy { get; set; }
        DateTime UpdatedOn { get; set; }
    }
}