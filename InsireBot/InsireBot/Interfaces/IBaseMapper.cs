using Maple.Core;
using Maple.Data;

namespace Maple
{
    public interface IBaseMapper<TVieModel, TCoreModel, TDataModel>
        where TVieModel : BaseDataViewModel<TVieModel, TDataModel>
        where TDataModel : BaseObject
    {
        TVieModel Get(TDataModel model);
        TVieModel Get(TCoreModel dto);

        TDataModel GetData(TVieModel viewModel);
        TDataModel GetData(TCoreModel dto);

        TCoreModel GetCore(TVieModel viewModel);
        TCoreModel GetCore(TDataModel model);
    }
}
