namespace Maple.Domain
{
    public static class MediaItemExtensions
    {
        public static MediaItemModel GetModel(this IMediaItem instance)
        {
            return new MediaItemModel()
            {
                Id = instance.Id,
                Name = instance.Name,
                Sequence = instance.Sequence,

                Duration = instance.Duration,
                PrivacyStatus = instance.PrivacyStatus,
                MediaItemType = instance.MediaItemType,

                CreatedBy = instance.CreatedBy,
                CreatedOn = instance.CreatedOn,
                UpdatedBy = instance.UpdatedBy,
                UpdatedOn = instance.UpdatedOn,
            };
        }
    }
}
