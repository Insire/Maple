using Microsoft.Extensions.Logging;
using MvvmScarletToolkit.Abstractions;

namespace Maple.Domain
{
    public interface IMapleCommandBuilder : ICommandBuilder
    {
        IScarletMessenger Messenger { get; }
        ILoggerFactory Log { get; }
        ILocalizationService LocalizationService { get; }
        ISequenceService SequenceService { get; }
    }
}
