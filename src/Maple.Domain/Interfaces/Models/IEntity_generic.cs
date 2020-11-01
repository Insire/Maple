namespace Maple.Domain
{
    public interface IEntity<TKey> : IEntity
    {
        TKey Id { get; }

        public static bool IsNew<T>(IEntity<TKey> entity)
            where T : TKey
        {
            return entity.Id.Equals(default(TKey));
        }
    }
}
