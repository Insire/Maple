using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

using Maple.Domain;

namespace Maple.Core
{
    // TODO add logic for handling INotifyDataErrorInfo for children and on this
    public abstract class ValidableBaseDataListViewModel<TViewModel, TModel, TKeyDataType> : BaseDataListViewModel<TViewModel, TModel, TKeyDataType>, ILoadableViewModel, IVirtualizedListViewModel
        where TViewModel : VirtualizationViewModel<TModel, TKeyDataType>, ISequence
        where TModel : class, IEntity, new()
    {
        public bool IsLoaded { get; protected set; }

        public IAsyncCommand LoadCommand { get; }
        public IAsyncCommand RefreshCommand { get; }

        public ICommand AddCommand { get; }

        protected ValidableBaseDataListViewModel(ViewModelServiceContainer container)
            : base(container)
        {
            AddCommand = new RelayCommand(Add, CanAdd);
            LoadCommand = AsyncCommand.Create(Load, CanLoad);
            RefreshCommand = AsyncCommand.Create(Refresh, CanRefresh);
        }

        private void Add()
        {
            // create new instance
            // add to repo
            // wrap in viewmodel
            // add to internal list
        }

        private bool CanAdd()
        {
            return Items != null;
        }

        private Task Load(CancellationToken token)
        {
            throw new NotImplementedException();
        }

        private bool CanLoad()
        {
            return !IsLoaded && !IsBusy;
        }

        private Task Refresh()
        {
            throw new NotImplementedException();
        }

        private bool CanRefresh()
        {
            return !IsBusy;
        }

        protected Task<int> GetCountAsync()
        {
            return Repository.GetCountAsync<TModel>();
        }

        protected async Task<List<IEntity>> GetItemsWithKey(ICollection<object> ids)
        {
            return await Repository.GetByIdsAsync<TModel>(ids);
        }

        public void ExtendItems(IEnumerable<object> items)
        {
            throw new System.NotImplementedException();
        }

        public void DeflateItem(object item)
        {
            throw new System.NotImplementedException();
        }
    }
}
