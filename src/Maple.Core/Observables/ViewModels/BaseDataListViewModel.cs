using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Maple.Domain;
using Maple.Localization.Properties;

namespace Maple.Core
{
    /// <summary>
    /// ListViewModel implementation for ObservableObjects related to the DataAccessLayer (DB)
    /// </summary>
    /// <typeparam name="TViewModel">a wrapper class implementing <see cref="BaseViewModel" /></typeparam>
    /// <typeparam name="TModel">a DTO implementing <see cref="BaseObject" /></typeparam>
    /// <seealso cref="Maple.Core.BaseListViewModel{T}" />
    public abstract class BaseDataListViewModel<TViewModel, TModel, TKeyDataType> : BaseListViewModel<TViewModel>, ILoadableViewModel, ISaveableViewModel
        where TViewModel : BaseDataViewModel<TViewModel, TModel, TKeyDataType>, ISequence
        where TModel : class, IBaseModel<TKeyDataType>
    {
        protected readonly ISequenceService _sequenceProvider;
        protected readonly ILocalizationService _translationService;
        protected readonly ILoggingService _log;

        public abstract bool IsLoaded { get; protected set; }

        public abstract IAsyncCommand SaveCommand { get; }
        public abstract IAsyncCommand LoadCommand { get; }
        public abstract IAsyncCommand RefreshCommand { get; }

        public abstract Task LoadAsync();
        public abstract Task SaveAsync();

        protected BaseDataListViewModel(ViewModelServiceContainer container)
            : base(container.Messenger)
        {
            _log = container.Log;
            _translationService = container.LocalizationService;
            _sequenceProvider = container.SequenceService;
        }

        /// <summary>
        /// Removes the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        public override void Remove(TViewModel viewModel)
        {
            if (viewModel == null)
                throw new ArgumentNullException(nameof(viewModel), $"{nameof(viewModel)} {Resources.IsRequired}");

            using (BusyStack.GetToken())
            {
                while (Items.Contains(viewModel))
                {
                    viewModel.Model.IsDeleted = true;
                    base.Remove(viewModel);
                }
            }
        }

        /// <summary>
        /// Removes the range.
        /// </summary>
        /// <param name="items">The items.</param>
        public override void RemoveRange(IEnumerable<TViewModel> items)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items), $"{nameof(items)} {Resources.IsRequired}");

            using (BusyStack.GetToken())
            {
                items.ForEach(p => p.Model.IsDeleted = true);
                base.RemoveRange(items);
            }
        }

        /// <summary>
        /// Removes the range.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <exception cref="System.ArgumentNullException">items</exception>
        public override void RemoveRange(IList items)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items), $"{nameof(items)} {Resources.IsRequired}");

            using (BusyStack.GetToken())
            {
                foreach (var item in items)
                    Remove(item as TViewModel);
            }
        }

        public override void Add(TViewModel viewModel)
        {
            if (viewModel == null)
                throw new ArgumentNullException(nameof(viewModel), $"{nameof(viewModel)} {Resources.IsRequired}");

            var sequence = _sequenceProvider.Get(Items.Cast<ISequence>().ToList());
            viewModel.Sequence = sequence;
            base.Add(viewModel);
        }

        public override void AddRange(IEnumerable<TViewModel> items)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items), $"{nameof(items)} {Resources.IsRequired}");

            using (BusyStack.GetToken())
            {
                var added = false;
                var sequence = _sequenceProvider.Get(Items.Cast<ISequence>().ToList());

                foreach (var item in items)
                {
                    item.Sequence = sequence;
                    Add(item);

                    sequence++;
                    added = true;
                }

                if (SelectedItem == null && added)
                    SelectedItem = Items.First();
            }
        }


    }
}
