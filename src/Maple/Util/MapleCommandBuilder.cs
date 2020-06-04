using System;
using System.ComponentModel;
using Maple.Domain;
using Microsoft.Extensions.Logging;
using MvvmScarletToolkit;
using MvvmScarletToolkit.Abstractions;

namespace Maple
{
    public sealed class MapleCommandBuilder : ScarletCommandBuilder, IMapleCommandBuilder
    {
        public ILocalizationService LocalizationService { get; }
        public ISequenceService SequenceService { get; }
        public ILoggerFactory Log { get; }

        public MapleCommandBuilder(IScarletDispatcher dispatcher
            , IScarletCommandManager commandManager
            , IScarletMessenger messenger
            , IExitService exitService
            , IScarletEventManager<INotifyPropertyChanged, PropertyChangedEventArgs> weakEventManager
            , Func<Action<bool>, IBusyStack> busyStackFactory
            , ILoggerFactory log
            , ILocalizationService localizationService
            , ISequenceService sequenceService)
            : base(dispatcher, commandManager, messenger, exitService, weakEventManager, busyStackFactory)
        {
            Log = log ?? throw new ArgumentNullException(nameof(log));
            LocalizationService = localizationService ?? throw new ArgumentNullException(nameof(localizationService));
            SequenceService = sequenceService ?? throw new ArgumentNullException(nameof(sequenceService));
        }
    }
}
