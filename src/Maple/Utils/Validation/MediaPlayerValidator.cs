using FluentValidation;
using MvvmScarletToolkit.Abstractions;

namespace Maple
{
    public class MediaPlayerValidator : BaseValidator<MediaPlayer>, IValidator<MediaPlayer>
    {
        public MediaPlayerValidator(ILocalizationService translationService)
            : base(translationService)
        {
            RuleFor(mediaPlayer => mediaPlayer.Name).NotEmpty(); // TODO finalize Validator configuration

            RuleFor(mediaPlayer => mediaPlayer.AudioDevices).NotEmpty();
        }
    }
}
