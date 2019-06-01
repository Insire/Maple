using FluentValidation;
using MvvmScarletToolkit.Abstractions;

namespace Maple
{
    public class MediaPlayersValidator : BaseValidator<MediaPlayers>, IValidator<MediaPlayers>
    {
        public MediaPlayersValidator(ILocalizationService translationService)
            : base(translationService)
        {
        }
    }
}
