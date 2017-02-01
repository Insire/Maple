namespace InsireBot.Data
{
    public class MediaItem : DatabaseObject
    {
        public string Title { get; set; }
        /// <summary>
        /// Seconds (?)
        /// </summary>
        public int Duration { get; set; }

        public string Location{ get; set; }
        public int PrivacyStatus { get; set; }
    }
}
