using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Maple.Domain;

namespace Maple.Core
{
    public abstract class VirtualizationListViewModel<TViewModel, TModel, TKeyDataType> : ValidableBaseDataListViewModel<TViewModel, TModel, TKeyDataType>, IVirtualizedViewModel, ILoadableViewModel, ISaveableViewModel
        where TViewModel : BaseDataViewModel<TViewModel, TModel, TKeyDataType>, ISequence
        where TModel : class, IBaseObject<TKeyDataType>
    {
        private bool _isLoaded;
        /// <summary>
        /// Indicates whether the LoadCommand/ the Load Method has been executed yet
        /// </summary>
        public override bool IsLoaded
        {
            get { return _isLoaded; }
            protected set
            {
                if (value)
                    SetValue(ref _isLoaded, value, OnChanged: () => Messenger.Publish(new LoadedMessage(this, this)));
            }
        }

        public int Count => Items?.Count ?? 0;

        /// <summary>
        /// Gets the load command.
        /// </summary>
        /// <value>
        /// The load command.
        /// </value>
        public override IAsyncCommand LoadCommand => AsyncCommand.Create(LoadAsync, () => !IsLoaded);
        /// <summary>
        /// Gets the refresh command.
        /// </summary>
        /// <value>
        /// The refresh command.
        /// </value>
        public override IAsyncCommand RefreshCommand => AsyncCommand.Create(LoadAsync);
        /// <summary>
        /// Gets the save command.
        /// </summary>
        /// <value>
        /// The save command.
        /// </value>
        public override IAsyncCommand SaveCommand => AsyncCommand.Create(SaveAsync);

        protected VirtualizationListViewModel(ViewModelServiceContainer container)
            : base(container)
        {
            var items = new RangeObservableCollection<TViewModel>();
            items.CollectionChanged += ItemsCollectionChanged;
            Items = items;

            View = new VirtualizingCollectionViewSource(items);

            // initial Notification, so that UI recognizes the value
            OnPropertyChanged(nameof(Count));
        }

        public void DeflateItem(object item)
        {
            throw new NotImplementedException();
        }

        public void ExtendItems(IEnumerable<object> items)
        {
            throw new NotImplementedException();
        }

        private void ItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(Count));
        }
    }
}
