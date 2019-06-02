using FluentValidation;
using MvvmScarletToolkit.Abstractions;

namespace Maple
{
    public class PlaylistsValidator : BaseValidator<Playlists>
    {
        public PlaylistsValidator(ILocalizationService translationService, IValidator<Playlist> playlistValidator)
            : base(translationService)
        {
            RuleFor(playlists => playlists.Items).NotNull();
            RuleForEach(playlists => playlists.Items).SetValidator(playlistValidator);
        }
    }
}