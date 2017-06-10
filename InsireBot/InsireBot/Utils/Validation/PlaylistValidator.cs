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
}
