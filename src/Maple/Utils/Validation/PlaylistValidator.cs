using FluentValidation;
using MvvmScarletToolkit.Abstractions;

namespace Maple
{
    public class PlaylistValidator : BaseValidator<Playlist>, IValidator<Playlist>
    {
        public PlaylistValidator(ILocalizationService translationService, IValidator<MediaItem> mediaItemValidator)
            : base(translationService)
        {
            RuleFor(playlist => playlist.Title).NotEmpty();
            RuleFor(playlist => playlist.Title).Length(1, 1024);

            RuleFor(playlist => playlist.Description).NotEmpty();
            RuleFor(playlist => playlist.Description).Length(0, 1024);

            RuleFor(playlist => playlist.PrivacyStatus).NotNull();

            RuleFor(playlist => playlist.RepeatModes).NotEmpty();
            RuleFor(playlist => playlist.RepeatMode).NotNull();

            RuleFor(playlist => playlist.MediaItems).NotNull();
            RuleForEach(playlist => playlist.MediaItems.Items).SetValidator(mediaItemValidator);
        }
    }
}
