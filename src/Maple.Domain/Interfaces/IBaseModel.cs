using System;

namespace Maple.Domain
{
    public interface IBaseModel<TKeyDataType>
    {
        string CreatedBy { get; set; }
        DateTime CreatedOn { get; set; }
        TKeyDataType Id { get; set; }
        bool IsDeleted { get; set; }
        bool IsNew { get; }
        byte[] RowVersion { get; set; }
        int Sequence { get; set; }
        string UpdatedBy { get; set; }
        DateTime UpdatedOn { get; set; }
    }
}