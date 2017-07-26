using Maple.Core;
using Maple.Localization.Properties;
using System;

namespace Maple
{
    public class FileBrowserDialogViewModel : ObservableObject
    {
        private FileSystemBrowserOptions _options;

        public FileSystemViewModel FileSystemViewModel { get; private set; }

        public FileBrowserDialogViewModel(FileSystemViewModel fileSystemViewModel, FileSystemBrowserOptions options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options), $"{nameof(options)} {Resources.IsRequired}");
            FileSystemViewModel = fileSystemViewModel ?? throw new ArgumentNullException(nameof(fileSystemViewModel), $"{nameof(fileSystemViewModel)} {Resources.IsRequired}");
        }
    }
}
