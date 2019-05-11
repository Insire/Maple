using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

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
    public abstract class BaseDataListViewModel<TViewModel, TModel> : ViewModelListBase<TViewModel>, ILoadableViewModel
        where TViewModel : ValidableBaseDataViewModel<TViewModel, TModel>, ISequence
        where TModel : class, IBaseObject
    {
        protected readonly ISequenceService _sequenceProvider;

        /// <summary>
        /// Gets the save command.
        /// </summary>
        /// <value>
        /// The save command.
        /// </value>
        public ICommand SaveCommand => AsyncCommand.Create(Save, CanSave);

        protected BaseDataListViewModel(ViewModelServiceContainer container)
            : base(container.Messenger)
        {
            _sequenceProvider = container.SequenceService;
        }

        public abstract Task Load();

        public abstract Task Save();

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
