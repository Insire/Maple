namespace Maple.Domain
{
    public static class MediaPlayerExtensions
    {
        public static MediaPlayerModel GetModel(this IMediaPlayer instance)
        {
            return new MediaPlayerModel()
            {
                Id = instance.Id,
                Name = instance.Name,
                Sequence = instance.Sequence,

                AudioDeviceId = instance.AudioDeviceId,
                IsPrimary = instance.IsPrimary,
                PlaylistId = instance.PlaylistId,

                CreatedBy = instance.CreatedBy,
                CreatedOn = instance.CreatedOn,
                UpdatedBy = instance.UpdatedBy,
                UpdatedOn = instance.UpdatedOn,
            };
        }
    }
}
