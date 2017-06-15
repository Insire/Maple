using Maple.Localization.Properties;
using System;

namespace Maple.Core
{
    public class ViewModelServiceContainer
    {
        public ILoggingNotifcationService NotificationService { get; }
        public ILocalizationService LocalizationService { get; }
        public ISequenceService SequenceService { get; }
        public IMessenger Messenger { get; }
        public ILoggingService Log { get; }

        public ViewModelServiceContainer(ILoggingService log, ILoggingNotifcationService notificationService, ILocalizationService localizationService, IMessenger messenger, ISequenceService sequenceService)
        {
            Log = log ?? throw new ArgumentNullException(nameof(log), $"{nameof(log)} {Resources.IsRequired}");
            LocalizationService = localizationService ?? throw new ArgumentNullException(nameof(localizationService), $"{nameof(localizationService)} {Resources.IsRequired}");
            SequenceService = sequenceService ?? throw new ArgumentNullException(nameof(sequenceService), $"{nameof(sequenceService)} {Resources.IsRequired}");
            Messenger = messenger ?? throw new ArgumentNullException(nameof(messenger), $"{nameof(messenger)} {Resources.IsRequired}");
            NotificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService), $"{nameof(notificationService)} {Resources.IsRequired}");
        }
    }
}
