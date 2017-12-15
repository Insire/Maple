using System;
using Maple.Core;
using Maple.Localization.Properties;

namespace Maple
{
    public class FileBrowserContentDialogViewModel : ObservableObject
    {
        private FileSystemBrowserOptions _options;

        private FileSystemViewModel _fileSystemViewModel;
        public FileSystemViewModel FileSystemViewModel
        {
            get { return _fileSystemViewModel; }
            private set { SetValue(ref _fileSystemViewModel, value); }
        }

        public FileBrowserContentDialogViewModel(FileSystemViewModel fileSystemViewModel, FileSystemBrowserOptions options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options), $"{nameof(options)} {Resources.IsRequired}");
            FileSystemViewModel = fileSystemViewModel ?? throw new ArgumentNullException(nameof(fileSystemViewModel), $"{nameof(fileSystemViewModel)} {Resources.IsRequired}");
        }
    }
}
