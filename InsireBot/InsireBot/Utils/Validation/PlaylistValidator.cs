using FluentValidation;
using Maple.Core;

namespace Maple
{
    public class PlaylistValidator : BaseValidator<Playlist>, IValidator<Playlist>
    {
        public PlaylistValidator(ILocalizationService translationService, IValidator<MediaItem> mediaItemValidator)
            : base(translationService)
        {
            RuleFor(playlist => playlist.Title).NotEmpty();
            RuleFor(playlist => playlist.Description).NotEmpty();
            RuleFor(playlist => playlist.SelectedItem).NotEmpty();
            RuleFor(playlist => playlist.RepeatModes).NotEmpty();
            RuleFor(playlist => playlist.RepeatModes).NotEmpty();

            RuleFor(playlist => playlist.Items).NotEmpty()
                                               .SetCollectionValidator(mediaItemValidator);
        }
    }

    public class MediaItemValidator : BaseValidator<MediaItem>, IValidator<MediaItem>
    {
        public MediaItemValidator(ILocalizationService translationService)
            : base(translationService)
        {
            RuleFor(mediaItem => mediaItem.Title).NotEmpty();
            RuleFor(mediaItem => mediaItem.Description).NotEmpty();
            RuleFor(mediaItem => mediaItem.Location).NotEmpty();
        }
    }

    public class MediaPlayerValidator : BaseValidator<MediaPlayer>, IValidator<MediaPlayer>
    {
        public MediaPlayerValidator(ILocalizationService translationService)
            : base(translationService)
        {
            RuleFor(mediaPlayer => mediaPlayer.AudioDevices).NotEmpty();
            RuleFor(mediaPlayer => mediaPlayer.Model).NotEmpty();
            RuleFor(mediaPlayer => mediaPlayer.Name).NotEmpty();
            RuleFor(mediaPlayer => mediaPlayer.Player).NotEmpty();
            RuleFor(mediaPlayer => mediaPlayer.Playlist).NotEmpty();
        }
    }

    public class PlaylistsValidator : BaseValidator<Playlists>, IValidator<Playlists>
    {
        public PlaylistsValidator(ILocalizationService translationService, IValidator<Playlist> playlistValidator)
            : base(translationService)
        {
            RuleFor(playlists => playlists.Items).NotEmpty()
                                                 .SetCollectionValidator(playlistValidator);
        }
    }
}
