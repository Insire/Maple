using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Maple.Domain;
using MvvmScarletToolkit.Abstractions;
using MvvmScarletToolkit.Observables;

namespace Maple.Core
{
    /// <summary>
    /// Generic CollectionViewModelBase that provides common services required in Maple
    /// </summary>
    public abstract class MapleBusinessViewModelListBase<TViewModel, TModel> : MapleBusinessViewModelListBase<TViewModel>
        where TViewModel : MapleDomainViewModelBase<TModel>, ISequence
        where TModel : class, IBaseObject
    {
        private TModel _model;
        [Bindable(true, BindingDirection.OneWay)]
        public TModel Model
        {
            get { return _model; }
            protected set { SetValue(ref _model, value); }
        }

        protected MapleBusinessViewModelListBase(IMapleCommandBuilder commandBuilder)
            : base(commandBuilder)
        {
        }

        public override async Task Remove(TViewModel item)
        {
            using (BusyStack.GetToken())
            {
                while (Items.Contains(item))
                {
                    await base.Remove(item);
                    item.Model.IsDeleted = true;
                }
            }
        }

        public override async Task Add(TViewModel item)
        {
            await base.Add(item);

            item.Sequence = SequenceService.Get(Items.Cast<ISequence>().ToList());
        }
    }

    /// <summary>
    /// Collection ViewModelBase that provides common services required in Maple
    /// </summary>
    public abstract class MapleBusinessViewModelListBase<TViewModel> : BusinessViewModelListBase<TViewModel>
        where TViewModel : class, INotifyPropertyChanged
    {
        protected readonly IMessenger Messenger;
        protected readonly ILoggingService Log;
        protected readonly ILoggingNotifcationService NotificationService;
        protected readonly ILocalizationService LocalizationService;
        protected readonly ISequenceService SequenceService;

        protected MapleBusinessViewModelListBase(IMapleCommandBuilder commandBuilder)
            : base(commandBuilder)
        {
            Messenger = commandBuilder.Messenger ?? throw new ArgumentNullException(nameof(Messenger));
            Log = commandBuilder.Log ?? throw new ArgumentNullException(nameof(Log));
            NotificationService = commandBuilder.NotificationService ?? throw new ArgumentNullException(nameof(NotificationService));
            LocalizationService = commandBuilder.LocalizationService ?? throw new ArgumentNullException(nameof(LocalizationService));
            SequenceService = commandBuilder.SequenceService ?? throw new ArgumentNullException(nameof(SequenceService));
        }
    }
}
