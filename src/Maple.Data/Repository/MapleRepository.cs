namespace Maple.Data
{
    public sealed class MapleRepository : EntityFrameworkRepository<MapleContext>
    {
        public MapleRepository(MapleContext context)
            : base(context)
        {
        }
    }
}
