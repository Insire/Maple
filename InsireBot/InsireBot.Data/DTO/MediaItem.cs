namespace Maple.Data
{
    public class MediaItem : DatabaseObject
    {
        public string Title { get; set; }
        /// <summary>
        /// Seconds (?)
        /// </summary>
        public long Duration { get; set; }

        public string Location{ get; set; }
        public int PrivacyStatus { get; set; }
    }
}
