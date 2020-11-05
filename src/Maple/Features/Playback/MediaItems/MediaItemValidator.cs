using FluentValidation;
using MvvmScarletToolkit.Abstractions;

namespace Maple
{
    internal sealed class MediaItemValidator : BaseValidator<MediaItem>, IValidator<MediaItem>
    {
        public MediaItemValidator(ILocalizationService translationService)
            : base(translationService)
        {
            RuleFor(mediaItem => mediaItem.Name).NotEmpty(); // TODO finalize Validator configuration
            RuleFor(mediaItem => mediaItem.Thumbnail).NotEmpty().Length(0, 260); // max windows path length https://docs.microsoft.com/en-us/windows/win32/fileio/naming-a-file
            RuleFor(mediaItem => mediaItem.Location).NotEmpty().Length(0, 260); // max windows path length https://docs.microsoft.com/en-us/windows/win32/fileio/naming-a-file
        }
    }
}
