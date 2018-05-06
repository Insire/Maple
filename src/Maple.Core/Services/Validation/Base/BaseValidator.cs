using System;

using FluentValidation;

using Maple.Domain;
using Maple.Localization.Properties;

namespace Maple.Core
{
    public abstract class BaseValidator<T> : AbstractValidator<T>
    {
        protected readonly ILocalizationService _translationService;

        protected BaseValidator(ILocalizationService translationService)
        {
            _translationService = translationService ?? throw new ArgumentNullException(nameof(translationService), $"{nameof(translationService)} {Resources.IsRequired}");
        }
    }
}
