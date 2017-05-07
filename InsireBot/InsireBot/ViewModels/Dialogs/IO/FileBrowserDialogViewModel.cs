using Maple.Core;
using System;

namespace Maple
{
    public class FileBrowserDialogViewModel : ObservableObject
    {
        private FileSystemBrowserOptions _options;

        public FileSystemViewModel FileSystemViewModel { get; private set; }

        public FileBrowserDialogViewModel(FileSystemViewModel fileSystemViewModel, FileSystemBrowserOptions options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
            FileSystemViewModel = fileSystemViewModel ?? throw new ArgumentNullException(nameof(fileSystemViewModel));
        }
    }
}
