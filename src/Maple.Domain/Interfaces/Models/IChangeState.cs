namespace Maple.Domain
{
    public interface IChangeState
    {
        bool IsNew { get; }

        bool IsDeleted { get; }
    }
}
