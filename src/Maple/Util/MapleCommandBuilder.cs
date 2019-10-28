using System;
using System.ComponentModel;
using Maple.Domain;
using Microsoft.Extensions.Logging;
using MvvmScarletToolkit;
using MvvmScarletToolkit.Abstractions;
using MvvmScarletToolkit.Commands;

namespace Maple
{
    public class MapleCommandBuilder : CommandBuilder, IMapleCommandBuilder
    {
        public ILocalizationService LocalizationService { get; }
        public ISequenceService SequenceService { get; }
        public ILoggerFactory Log { get; }
        public Func<IUnitOfWork> ContextFactory { get; }

        public MapleCommandBuilder(IScarletDispatcher dispatcher
            , IScarletCommandManager commandManager
            , IScarletMessenger messenger
            , IExitService exitService
            , IWeakEventManager<INotifyPropertyChanged, PropertyChangedEventArgs> weakEventManager
            , Func<Action<bool>, IBusyStack> busyStackFactory
            , ILoggerFactory log
            , ILocalizationService localizationService
            , ISequenceService sequenceService
            , Func<IUnitOfWork> contextFactory)
            : base(dispatcher, commandManager, messenger, exitService, weakEventManager, busyStackFactory)
        {
            Log = log ?? throw new ArgumentNullException(nameof(log));
            LocalizationService = localizationService ?? throw new ArgumentNullException(nameof(localizationService));
            SequenceService = sequenceService ?? throw new ArgumentNullException(nameof(sequenceService));
            ContextFactory = contextFactory ?? throw new ArgumentNullException(nameof(contextFactory));
        }
    }
}
