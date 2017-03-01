namespace Maple.Data
{
    public class MediaPlayer : IModel
    {
        public int Id { get; set; }
        public int Sequence { get; set; }
        public int PlaylistId { get; set; }
        public string Name { get; set; }
        public string DeviceName { get; set; }
        public bool IsPrimary { get; set; }
    }
}
