using AutoMapper;
using FluentValidation;
using Maple.Core;
using System;

namespace Maple
{
    public abstract class BaseMapper<T>
    {
        protected IMapper _mapper;
        protected readonly ILocalizationService _translationService;
        protected readonly ISequenceProvider _sequenceProvider;
        protected readonly IMapleLog _log;
        protected readonly IValidator<T> _validator;

        public BaseMapper(ILocalizationService translationService, ISequenceProvider sequenceProvider, IMapleLog log, IValidator<T> validator)
        {
            _translationService = translationService ?? throw new ArgumentNullException(nameof(translationService));
            _sequenceProvider = sequenceProvider ?? throw new ArgumentNullException(nameof(sequenceProvider));
            _log = log ?? throw new ArgumentNullException(nameof(log));
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        }

        protected abstract void InitializeMapper();
    }
}
