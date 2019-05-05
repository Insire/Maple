namespace Maple.Domain
{
    public interface IEntity<out TKey> : IBaseObject
    {
        TKey Id { get; }
    }
}
