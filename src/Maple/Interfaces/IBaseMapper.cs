using Maple.Core;
using Maple.Data;

namespace Maple
{
    public interface IBaseMapper<TVieModel, TDataModel>
        where TVieModel : BaseDataViewModel<TVieModel, TDataModel>
        where TDataModel : BaseObject
    {
        TVieModel Get(TDataModel model);

        TDataModel GetData(TVieModel viewModel);
    }
}
