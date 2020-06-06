namespace Maple.Domain
{
    public interface IPlaylist : ISequence
    {
        bool IsShuffeling { get; set; }
        PrivacyStatus PrivacyStatus { get; }
        RepeatMode RepeatMode { get; set; }
        string Thumbnail { get; set; }
        string Name { get; set; }
    }
}
