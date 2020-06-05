using FluentValidation;
using MvvmScarletToolkit.Abstractions;

namespace Maple
{
    internal sealed class PlaylistValidator : BaseValidator<Playlist>, IValidator<Playlist>
    {
        public PlaylistValidator(ILocalizationService translationService, IValidator<MediaItem> mediaItemValidator)
            : base(translationService)
        {
            RuleFor(playlist => playlist.Title).NotEmpty();
            RuleFor(playlist => playlist.Title).Length(1, 1024);

            RuleFor(playlist => playlist.Thumbnail).NotEmpty();
            RuleFor(playlist => playlist.Thumbnail).Length(0, 260); // max windows path length https://docs.microsoft.com/en-us/windows/win32/fileio/naming-a-file

            RuleFor(playlist => playlist.PrivacyStatus).NotNull();

            RuleFor(playlist => playlist.RepeatMode).NotNull();

            RuleFor(playlist => playlist.Items).NotNull();
            RuleForEach(playlist => playlist.Items).SetValidator(mediaItemValidator);
        }
    }
}
