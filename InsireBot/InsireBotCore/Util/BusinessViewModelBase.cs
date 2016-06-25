namespace InsireBotCore
{
    /// <summary>
    /// handles data related to data logic and data flow throughout the application
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BusinessViewModelBase<T> : DataViewModelBase<T> where T : IIsSelected, IIndex, IIdentifier
    {
        protected readonly IDataService _dataService;

        public BusinessViewModelBase(IDataService dataService) : base()
        {
            _dataService = dataService;
        }
    }
}
