using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Maple.Domain;
using Maple.Localization.Properties;

namespace Maple.Core
{
    // TODO add virtualization here
    //
    public abstract class BaseDataListViewModel<TViewModel, TModel, TKeyDataType> : BaseListViewModel<TViewModel>, ILoadableViewModel<TKeyDataType>, ISaveableViewModel
        where TViewModel : VirtualizationViewModel<TViewModel, TModel, TKeyDataType>, ISequence
        where TModel : class, IBaseModel<TKeyDataType>
    {
        private readonly IMapleRepository<TModel, TKeyDataType> _repository;

        protected readonly ISequenceService _sequenceProvider;
        protected readonly ILocalizationService _translationService;
        protected readonly ILoggingService _log;

        public abstract bool IsLoaded { get; protected set; }

        public abstract IAsyncCommand SaveCommand { get; }
        public abstract IAsyncCommand LoadCommand { get; }
        public abstract IAsyncCommand RefreshCommand { get; }

        public abstract Task GetCountAsync();
        public abstract Task SaveAsync();
        public abstract Task GetItemsWithKey(TKeyDataType[] keys);

        private ICollectionView _view;
        /// <summary>
        /// For grouping, sorting and filtering
        /// </summary>
        /// <value>
        /// The view.
        /// </value>
        public ICollectionView View
        {
            get { return _view; }
            protected set { SetValue(ref _view, value); }
        }

        protected BaseDataListViewModel(ViewModelServiceContainer container, IMapleRepository<TModel, TKeyDataType> repository)
            : base(container.Messenger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository), $"{nameof(repository)} {Resources.IsRequired}");

            _log = container.Log;
            _translationService = container.LocalizationService;
            _sequenceProvider = container.SequenceService;

            View = new VirtualizingCollectionViewSource(container, (IList)_items);
        }

        /// <summary>
        /// Removes the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        public override void Remove(TViewModel container)
        {
            if (container == null)
                throw new ArgumentNullException(nameof(container), $"{nameof(container)} {Resources.IsRequired}");

            using (BusyStack.GetToken())
            {
                while (Items.Contains(container))
                {
                    container.ViewModel.Model.IsDeleted = true;
                    base.Remove(container);
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
                items.ForEach(p => p.ViewModel.Model.IsDeleted = true);
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
