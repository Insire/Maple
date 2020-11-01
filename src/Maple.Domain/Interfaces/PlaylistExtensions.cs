namespace Maple.Domain
{
    public static class PlaylistExtensions
    {
        public static PlaylistModel GetModel(this IPlaylist instance)
        {
            return new PlaylistModel()
            {
                Id = instance.Id,
                Name = instance.Name,
                Sequence = instance.Sequence,

                Thumbnail = instance.Thumbnail,
                IsShuffeling = instance.IsShuffeling,
                PrivacyStatus = instance.PrivacyStatus,
                RepeatMode = instance.RepeatMode,

                CreatedBy = instance.CreatedBy,
                CreatedOn = instance.CreatedOn,
                UpdatedBy = instance.UpdatedBy,
                UpdatedOn = instance.UpdatedOn,
            };
        }
    }
}
