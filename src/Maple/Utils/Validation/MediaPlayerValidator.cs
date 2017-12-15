using FluentValidation;
using Maple.Core;

namespace Maple
{
    public class MediaPlayerValidator : BaseValidator<MediaPlayer>, IValidator<MediaPlayer>
    {
        public MediaPlayerValidator(ILocalizationService translationService)
            : base(translationService)
        {
            RuleFor(mediaPlayer => mediaPlayer.Name).NotEmpty(); // TODO finalize Validator configuration

            RuleFor(mediaPlayer => mediaPlayer.AudioDevices).NotEmpty();
            RuleFor(mediaPlayer => mediaPlayer.Model).NotEmpty();

            RuleFor(mediaPlayer => mediaPlayer.Player).NotEmpty();
            RuleFor(mediaPlayer => mediaPlayer.Playlist).NotEmpty();
        }
    }
}
