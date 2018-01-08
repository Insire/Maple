using Maple.Domain;

namespace Maple.Core
{
    public abstract class ValidableBaseDataListViewModel<TViewModel, TModel, TKeyDataType> : BaseDataListViewModel<TViewModel, TModel, TKeyDataType>
        where TViewModel : BaseDataViewModel<TViewModel, TModel, TKeyDataType>, ISequence
        where TModel : class, IBaseObject<TKeyDataType>
    {
        protected ValidableBaseDataListViewModel(ViewModelServiceContainer container)
            : base(container)
        {
        }

        // TODO add logic for handling INotifyDataErrorInfo for children and on this
    }
}
