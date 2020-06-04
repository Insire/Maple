using FluentValidation;
using MvvmScarletToolkit.Abstractions;

namespace Maple
{
    internal sealed class MediaItemValidator : BaseValidator<MediaItem>, IValidator<MediaItem>
    {
        public MediaItemValidator(ILocalizationService translationService)
            : base(translationService)
        {
            RuleFor(mediaItem => mediaItem.Title).NotEmpty(); // TODO finalize Validator configuration
            RuleFor(mediaItem => mediaItem.Description).NotEmpty();
            RuleFor(mediaItem => mediaItem.Location).NotEmpty();
        }
    }
}
