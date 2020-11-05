using FluentValidation;
using MvvmScarletToolkit.Abstractions;

namespace Maple
{
    internal sealed class MediaPlayersValidator : BaseValidator<MediaPlayers>, IValidator<MediaPlayers>
    {
        public MediaPlayersValidator(ILocalizationService translationService)
            : base(translationService)
        {
        }
    }
}
