namespace Maple.Data
{
    public class MediaPlayer : BaseObject
    {
        public int PlaylistId { get; set; }
        public string Name { get; set; }
        public string DeviceName { get; set; }
        public bool IsPrimary { get; set; }
    }
}
