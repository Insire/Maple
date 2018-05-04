using System;
using System.Runtime.Caching;

using Maple.Domain;
using Maple.Localization.Properties;

namespace Maple.Core
{
    public class ViewModelServiceContainer
    {
        public ILoggingNotifcationService NotificationService { get; }
        public ILocalizationService LocalizationService { get; }
        public ISequenceService SequenceService { get; }
        public IMessenger Messenger { get; }
        public ILoggingService Log { get; }
        public MemoryCache Cache { get; }
        public CacheItemPolicy CacheItemPolicy { get; }
        public IRepository Repository { get; }

        public ViewModelServiceContainer(ILoggingService log, IRepository repository,
            ILoggingNotifcationService notificationService, ILocalizationService localizationService,
            IMessenger messenger, ISequenceService sequenceService, MemoryCache cache, CacheItemPolicy itemPolicy)
        {
            Repository = repository ?? throw new ArgumentNullException(nameof(repository), $"{nameof(repository)} {Resources.IsRequired}");
            Log = log ?? throw new ArgumentNullException(nameof(log), $"{nameof(log)} {Resources.IsRequired}");
            LocalizationService = localizationService ?? throw new ArgumentNullException(nameof(localizationService), $"{nameof(localizationService)} {Resources.IsRequired}");
            SequenceService = sequenceService ?? throw new ArgumentNullException(nameof(sequenceService), $"{nameof(sequenceService)} {Resources.IsRequired}");
            Messenger = messenger ?? throw new ArgumentNullException(nameof(messenger), $"{nameof(messenger)} {Resources.IsRequired}");
            NotificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService), $"{nameof(notificationService)} {Resources.IsRequired}");
            Cache = cache ?? throw new ArgumentNullException(nameof(cache), $"{nameof(cache)} {Resources.IsRequired}");
            CacheItemPolicy = itemPolicy ?? throw new ArgumentNullException(nameof(itemPolicy), $"{nameof(itemPolicy)} {Resources.IsRequired}");
        }
    }
}
