using System;

using FluentValidation;
using Maple.Localization.Properties;
using MvvmScarletToolkit.Abstractions;

namespace Maple
{
    public abstract class BaseValidator<T> : AbstractValidator<T>, IValidator<T>
    {
        protected readonly ILocalizationService _translationService;

        protected BaseValidator(ILocalizationService translationService)
        {
            _translationService = translationService ?? throw new ArgumentNullException(nameof(translationService), $"{nameof(translationService)} {Resources.IsRequired}");
        }
    }
}
