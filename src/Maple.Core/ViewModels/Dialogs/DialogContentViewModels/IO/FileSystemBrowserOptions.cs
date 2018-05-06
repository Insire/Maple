namespace Maple.Core
{
    public class FileSystemBrowserOptions : ObservableObject
    {
        private bool _multiSelection;
        public bool MultiSelection
        {
            get { return _multiSelection; }
            set { SetValue(ref _multiSelection, value); }
        }

        private bool _canCancel;
        public bool CanCancel
        {
            get { return _canCancel; }
            set { SetValue(ref _canCancel, value); }
        }

        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetValue(ref _title, value); }
        }
    }
}
