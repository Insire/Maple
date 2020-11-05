using FluentValidation;
using MvvmScarletToolkit.Abstractions;

namespace Maple
{
    internal sealed class PlaylistValidator : BaseValidator<Playlist>, IValidator<Playlist>
    {
        public PlaylistValidator(ILocalizationService translationService, IValidator<MediaItem> mediaItemValidator)
            : base(translationService)
        {
            RuleFor(playlist => playlist.Name).NotEmpty();
            RuleFor(playlist => playlist.Name).Length(1, 64);

            RuleFor(playlist => playlist.PrivacyStatus).NotNull();

            RuleFor(playlist => playlist.RepeatMode).NotNull();

            RuleFor(playlist => playlist.Items).NotNull();
            RuleForEach(playlist => playlist.Items).SetValidator(mediaItemValidator);
        }
    }
}
