using AutoMapper;
using Maple.Core;
using System;

namespace Maple
{
    public abstract class BaseMapper
    {
        protected IMapper _mapper;
        protected readonly ITranslationService _translationService;
        protected readonly ISequenceProvider _sequenceProvider;
        protected readonly IMapleLog _log;

        public BaseMapper(ITranslationService translationService, ISequenceProvider sequenceProvider, IMapleLog log)
        {
            _translationService = translationService ?? throw new ArgumentNullException(nameof(translationService));
            _sequenceProvider = sequenceProvider ?? throw new ArgumentNullException(nameof(sequenceProvider));
            _log = log ?? throw new ArgumentNullException(nameof(log));
        }

        protected abstract void InitializeMapper();
    }
}
