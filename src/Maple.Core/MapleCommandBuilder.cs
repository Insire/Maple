using System;
using Maple.Domain;
using MvvmScarletToolkit.Abstractions;
using MvvmScarletToolkit.Commands;

namespace Maple.Core
{
    public class MapleCommandBuilder : CommandBuilder, IMapleCommandBuilder
    {
        public ILoggingNotifcationService NotificationService { get; }
        public ILocalizationService LocalizationService { get; }
        public ISequenceService SequenceService { get; }
        public IMessenger Messenger { get; }
        public ILoggingService Log { get; }

        public MapleCommandBuilder(ILoggingService log, ILoggingNotifcationService notificationService, ILocalizationService localizationService, IMessenger messenger, ISequenceService sequenceService, IScarletDispatcher dispatcher, IScarletCommandManager commandManager, Func<Action<bool>, IBusyStack> busyStackFactory)
            : base(dispatcher, commandManager, busyStackFactory)
        {
            Log = log ?? throw new ArgumentNullException(nameof(log));
            LocalizationService = localizationService ?? throw new ArgumentNullException(nameof(localizationService));
            SequenceService = sequenceService ?? throw new ArgumentNullException(nameof(sequenceService));
            Messenger = messenger ?? throw new ArgumentNullException(nameof(messenger));
            NotificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));
        }
    }
}
