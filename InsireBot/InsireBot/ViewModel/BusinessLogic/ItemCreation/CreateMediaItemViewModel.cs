using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;

namespace InsireBot
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
            set
            {
                if (_source != value)
                {
                    _source = value;
                    RaisePropertyChanged(nameof(Source));
                }
            }
        }

        private DataParsingServiceResult _result;
        public DataParsingServiceResult Result
        {
            get { return _result; }
            private set
            {
                if (_result != value)
                {
                    _result = value;
                    RaisePropertyChanged(nameof(Result));
                }
            }
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
                BusyStack.Push();
                Result = await GlobalServiceLocator.Instance.DataParsingService
                                                            .Parse(Source, DataParsingServiceResultType.MediaItems)
                                                            .ContinueWith((task) =>
                                                            {
                                                                if (!BusyStack.Pull())
                                                                    App.Log.Warn("Couldn't pull from BusyStack");

                                                                if (task.Exception != null)
                                                                    App.Log.Error(this, task.Exception);

                                                                var result = task.Result;

                                                                if (result.Count > 0)
                                                                {
                                                                    if (result.MediaItems?.Count > 0)
                                                                        Items.AddRange(result.MediaItems);
                                                                }

                                                                return result;
                                                            });
            });
        }
    }
}
