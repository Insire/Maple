namespace Maple.Domain
{
    public static class EntityExtensions
    {
        public static bool IsNew<TKey>(this IEntity<TKey> entity)
        {
            return entity.Id.Equals(default(TKey));
        }
    }
}
