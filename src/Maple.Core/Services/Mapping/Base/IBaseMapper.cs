using Maple.Domain;

namespace Maple.Core
{
    public interface IBaseMapper<TVieModel, TDataModel, TKeyDataType>
        where TVieModel : BaseDataViewModel<TVieModel, TDataModel, TKeyDataType>
        where TDataModel : class, IBaseModel<TKeyDataType>
    {
        TVieModel Get(TDataModel model);

        TDataModel GetData(TVieModel viewModel);
    }
}
