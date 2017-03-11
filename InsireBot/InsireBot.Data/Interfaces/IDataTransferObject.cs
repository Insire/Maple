namespace Maple.Data
{
    public interface IDataTransferObject
    {
        bool IsDeleted { get; }
        bool IsNew { get; }
    }
}
