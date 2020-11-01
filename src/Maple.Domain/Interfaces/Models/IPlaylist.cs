namespace Maple.Domain
{
    public interface IPlaylist : IEntity<int>
    {
        bool IsShuffeling { get; }
        PrivacyStatus PrivacyStatus { get; }
        RepeatMode RepeatMode { get; }
        string Thumbnail { get; }
    }
}
