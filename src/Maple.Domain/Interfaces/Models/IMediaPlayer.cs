namespace Maple.Domain
{
    public interface IMediaPlayer : IEntity<int>
    {
        int? AudioDeviceId { get; }
        bool IsPrimary { get; }
        int? PlaylistId { get; }
    }
}
