namespace Maple.Domain
{
    public interface IEntity<TKey> : IEntity
    {
        TKey Id { get; }

        public bool IsNew => Id.Equals(default(TKey));
    }
}
