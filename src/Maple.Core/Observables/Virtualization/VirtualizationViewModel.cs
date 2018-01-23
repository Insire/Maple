using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Maple.Domain;
using Maple.Localization.Properties;

namespace Maple.Core
{
    public class VirtualizationViewModel<TViewModel, TModel, TKeyDataType> : ObservableObject
        where TModel : class, IBaseModel<TKeyDataType>
    {
        private readonly IDataProvider<BaseDataViewModel<TViewModel, TModel, TKeyDataType>, TKeyDataType> _dataProvider;

        private TKeyDataType _id;
        public TKeyDataType Id
        {
            get { return _id; }
            private set { SetValue(ref _id, value); }
        }

        private bool _isExtended;
        public bool IsExtended
        {
            get { return _isExtended; }
            private set { SetValue(ref _isExtended, value); }
        }

        private VirtualizationViewModelState _state;
        public VirtualizationViewModelState State // TODO add implementation details
        {
            get { return _state; }
            private set { SetValue(ref _state, value); }
        }

        private bool _isBusy;
        public bool IsBusy
        {
            get { return _isBusy; }
            private set { SetValue(ref _isBusy, value); }
        }

        private BaseDataViewModel<TViewModel, TModel, TKeyDataType> _viewModel;
        public BaseDataViewModel<TViewModel, TModel, TKeyDataType> ViewModel
        {
            get { return _viewModel; }
            set { SetValue(ref _viewModel, value); }
        }

        private ICommand _deflateCommand;
        public ICommand DeflateCommand
        {
            get { return _deflateCommand; }
            private set { SetValue(ref _deflateCommand, value); }
        }

        private IAsyncCommand _expandCommand;
        public IAsyncCommand ExpandCommand
        {
            get { return _expandCommand; }
            private set { SetValue(ref _expandCommand, value); }
        }

        public VirtualizationViewModel(TKeyDataType id, IDataProvider<BaseDataViewModel<TViewModel, TModel, TKeyDataType>, TKeyDataType> dataProvider)
        {
            _id = id;
            _dataProvider = dataProvider ?? throw new ArgumentNullException(nameof(dataProvider), $"{nameof(dataProvider)} {Resources.IsRequired}");

            DeflateCommand = new RelayCommand(Deflate, CanDeflate);
            ExpandCommand = AsyncCommand.Create(Expand, CanExpand);
        }

        public async Task Expand()
        {
            if (ViewModel != null)
                return;

            ViewModel = await _dataProvider.Get(Id).ConfigureAwait(true);
            IsExtended = true;
        }

        public bool CanExpand()
        {
            return ViewModel == null && !IsExtended;
        }

        public void Deflate()
        {
            ViewModel = null; // enables GC
            IsExtended = false;
        }

        public bool CanDeflate()
        {
            return ViewModel != null && IsExtended;
        }
    }
}
