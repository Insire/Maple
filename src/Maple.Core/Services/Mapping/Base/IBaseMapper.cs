namespace Maple.Core
{
    public interface IBaseMapper<TVieModel, TDataModel, TKeyDataType>
        where TVieModel : BaseDataViewModel<TVieModel, TDataModel, TKeyDataType>
        where TDataModel : class
    {
        TVieModel Get(TDataModel model);

        TDataModel GetData(TVieModel viewModel);
    }
}
