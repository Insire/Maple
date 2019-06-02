using System;
using Maple.Domain;
using Microsoft.Extensions.Logging;
using MvvmScarletToolkit.Abstractions;
using MvvmScarletToolkit.Commands;

namespace Maple
{
    public class MapleCommandBuilder : CommandBuilder, IMapleCommandBuilder
    {
        public ILocalizationService LocalizationService { get; }
        public ISequenceService SequenceService { get; }
        public IScarletMessenger Messenger { get; }
        public ILoggerFactory Log { get; }

        public MapleCommandBuilder(ILoggerFactory log, ILocalizationService localizationService, IScarletMessenger messenger, ISequenceService sequenceService, IScarletDispatcher dispatcher, IScarletCommandManager commandManager, Func<Action<bool>, IBusyStack> busyStackFactory)
            : base(dispatcher, commandManager, busyStackFactory)
        {
            Log = log ?? throw new ArgumentNullException(nameof(log));
            LocalizationService = localizationService ?? throw new ArgumentNullException(nameof(localizationService));
            SequenceService = sequenceService ?? throw new ArgumentNullException(nameof(sequenceService));
            Messenger = messenger ?? throw new ArgumentNullException(nameof(messenger));
        }
    }
}
