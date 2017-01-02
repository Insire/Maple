using System.Windows.Input;
using MvvmScarletToolkit;

namespace InsireBotWPF
{
    /// <summary>
    /// viewmodel for creating mediaitem from a string (path/url)
    /// </summary>
    public class CreateMediaItemViewModel : MediaItemsStore
    {
        private string _source;
        public string Source
        {
            get { return _source; }
            set { SetValue(ref _source, value); }
        }

        private DataParsingServiceResult _result;
        public DataParsingServiceResult Result
        {
            get { return _result; }
            private set { SetValue(ref _result, value); }
        }

        public ICommand ParseCommand { get; private set; }

        public CreateMediaItemViewModel() : base()
        {
            InitializeCommands();
        }

        private void InitializeCommands()
        {
            ParseCommand = new RelayCommand(async () =>
            {
                using (BusyStack.GetToken())
                {
                    Result = await GlobalServiceLocator.Instance.DataParsingService.Parse(Source, DataParsingServiceResultType.MediaItems);

                    if (Result.Count > 0)
                    {
                        if (Result.MediaItems?.Count > 0)
                            Items.AddRange(Result.MediaItems);
                    }
                }
            });
        }
    }
}
