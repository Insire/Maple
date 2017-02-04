namespace Maple.Core
{
    public class MediaItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        /// <summary>
        /// Ticks
        /// </summary>
        public long Duration { get; set; }

        public string Location { get; set; }
        public int PrivacyStatus { get; set; }
        public int Sequence { get; set; }
        public int PlaylistId { get; set; }
    }
}
