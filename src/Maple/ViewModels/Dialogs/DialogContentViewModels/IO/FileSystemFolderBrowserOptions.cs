namespace Maple
{
    public class FileSystemFolderBrowserOptions : FileSystemBrowserOptions
    {
        private bool _includeSubFolders;
        public bool IncludeSubFolders
        {
            get { return _includeSubFolders; }
            set { SetValue(ref _includeSubFolders, value); }
        }
    }
}
