using AutoMapper;
using FluentValidation;
using Maple.Core;
using Maple.Localization.Properties;
using System;

namespace Maple
{
    public abstract class BaseMapper<T>
    {
        protected IMapper _mapper;
        protected readonly ILocalizationService _translationService;
        protected readonly ISequenceService _sequenceProvider;
        protected readonly ILoggingService _log;
        protected readonly IValidator<T> _validator;
        protected readonly IMessenger _messenger;
        protected readonly ViewModelServiceContainer _container;

        public BaseMapper(ViewModelServiceContainer container, IValidator<T> validator)
        {
            _container = container;
            _translationService = container.LocalizationService;
            _sequenceProvider = container.SequenceService;
            _log = container.Log;
            _messenger = container.Messenger;
            _validator = validator ?? throw new ArgumentNullException(nameof(validator), $"{nameof(validator)} {Resources.IsRequired}");
        }

        protected abstract void InitializeMapper();
    }
}
