using System;
using Microsoft.Extensions.Logging;
using MvvmScarletToolkit;
using MvvmScarletToolkit.Abstractions;

namespace Maple.Domain
{
    public interface IMapleCommandBuilder : ICommandBuilder
    {
        ILoggerFactory Log { get; }
        ILocalizationService LocalizationService { get; }
        ISequenceService SequenceService { get; }
        Func<IUnitOfWork> ContextFactory { get; }
    }
}
