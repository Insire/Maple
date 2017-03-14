namespace Maple.Core
{
    public interface IChangeState
    {
        bool IsNew { get; }
        bool IsDeleted { get; }
    }
}
