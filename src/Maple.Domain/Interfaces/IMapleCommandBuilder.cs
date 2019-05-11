using MvvmScarletToolkit.Abstractions;

namespace Maple.Domain
{
    public interface IMapleCommandBuilder : ICommandBuilder
    {
        IMessenger Messenger { get; }
        ILoggingService Log { get; }
        ILoggingNotifcationService NotificationService { get; }
        ILocalizationService LocalizationService { get; }
        ISequenceService SequenceService { get; }
    }
}
