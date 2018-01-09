using FluentValidation;

namespace Maple.Core
{
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
