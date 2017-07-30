﻿using FluentValidation;
using Maple.Core;
using Maple.Localization.Properties;
using System;

namespace Maple
{
    public abstract class BaseValidator<T> : AbstractValidator<T>, IValidator<T>
    {
        protected readonly ILocalizationService _translationService;

        public BaseValidator(ILocalizationService translationService)
        {
            _translationService = translationService ?? throw new ArgumentNullException(nameof(translationService), $"{nameof(translationService)} {Resources.IsRequired}");
        }
    }
}
