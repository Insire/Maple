namespace Maple.Data
{
    public class MediaItem : BaseObject
    {
        public string Title { get; set; }
        public string Description { get; set; }
        /// <summary>
        /// Ticks
        /// </summary>
        public long Duration { get; set; }

        public string Location { get; set; }
        public int PrivacyStatus { get; set; }

        public int PlaylistId { get; set; }
    }
}
