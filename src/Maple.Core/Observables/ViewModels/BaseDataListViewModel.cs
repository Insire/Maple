using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Maple.Domain;
using MvvmScarletToolkit.Observables;

namespace Maple.Core
{
    /// <summary>
    /// ListViewModel implementation for ObservableObjects related to the DataAccessLayer (DB)
    /// </summary>
    /// <typeparam name="TViewModel">a wrapper class implementing <see cref="BaseViewModel" /></typeparam>
    /// <typeparam name="TModel">a DTO implementing <see cref="BaseObject" /></typeparam>
    /// <seealso cref="Maple.Core.BaseListViewModel{T}" />
    public abstract class BusinessListViewModel<TViewModel, TModel> : ViewModelListBase<TViewModel>
        where TViewModel : ValidableBaseDataViewModel<TViewModel, TModel>, ISequence, INotifyPropertyChanged
        where TModel : class, IBaseObject
    {
        protected readonly ISequenceService _sequenceProvider;

        protected BusinessListViewModel(IMapleCommandBuilder commandBuilder)
            : base(commandBuilder)
        {
            _sequenceProvider = commandBuilder.SequenceService;
        }

        protected virtual bool CanSave()
        {
            return !Items.Any(p => p.HasErrors);
        }

        /// <summary>
        /// Removes the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
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

            item.Sequence = _sequenceProvider.Get(Items.Cast<ISequence>().ToList());
        }
    }
}
