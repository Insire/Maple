using FluentValidation;

using Maple.Domain;

namespace Maple.Core
{
    public class MediaItemValidator : BaseValidator<MediaItem>, IValidator<MediaItem>
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
