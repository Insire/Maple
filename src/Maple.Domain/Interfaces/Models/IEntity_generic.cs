namespace Maple.Domain
{
    public interface IEntity<out TKey> : IEntity
    {
        TKey Id { get; }
    }
}
