using FluentValidation;
using MvvmScarletToolkit.Abstractions;

namespace Maple
{
    internal sealed class MediaPlayerValidator : BaseValidator<MediaPlayer>, IValidator<MediaPlayer>
    {
        public MediaPlayerValidator(ILocalizationService translationService)
            : base(translationService)
        {
            RuleFor(mediaPlayer => mediaPlayer.Name).NotEmpty(); // TODO finalize Validator configuration
        }
    }
}
