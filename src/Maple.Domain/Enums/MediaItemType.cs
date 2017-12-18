namespace Maple.Domain
{
    public enum MediaItemType
    {
        None = 0,
        LocalFile = 1 << 0,
        Youtube = 1 << 1,
    }
}
