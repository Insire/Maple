namespace InsireBot.Data
{
    public class MediaItem : DatabaseObject
    {
        public string Title { get; set; }
        public int Duration { get; set; }

        public string Location{ get; set; }
        public bool IsRestricted { get; set; }
    }
}
