using Maple.Core;
using Maple.Interfaces;

namespace Maple
{
    public interface IBaseMapper<TVieModel, TDataModel>
        where TVieModel : BaseDataViewModel<TVieModel, TDataModel>
        where TDataModel : class, IBaseObject
    {
        TVieModel Get(TDataModel model);

        TDataModel GetData(TVieModel viewModel);
    }
}
