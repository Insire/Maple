using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

using Maple.Domain;
using Maple.Localization.Properties;

namespace Maple.Core
{
    // TODO add virtualization here
    //
    public abstract class BaseDataListViewModel<TViewModel, TModel, TKeyDataType> : BaseListViewModel<TViewModel>
        where TViewModel : VirtualizationViewModel<TViewModel, TModel, TKeyDataType>, ISequence
        where TModel : class, IEntity
    {
        protected readonly ISequenceService _sequenceProvider;
        protected readonly ILocalizationService _translationService;
        protected readonly ILoggingService _log;

        protected IRepository Repository { get; }

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

        protected BaseDataListViewModel(ViewModelServiceContainer container)
            : base(container.Messenger)
        {
            Repository = container.Repository;

            _log = container.Log;
            _translationService = container.LocalizationService;
            _sequenceProvider = container.SequenceService;

            View = new VirtualizingCollectionViewSource(container, Items);
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
                    Repository.Delete(container.ViewModel.Model);
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
                items.ForEach(p => Remove(p));
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
