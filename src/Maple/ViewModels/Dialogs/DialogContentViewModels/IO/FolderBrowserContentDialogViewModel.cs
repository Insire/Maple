using System;
using MvvmScarletToolkit.FileSystemBrowser;
using MvvmScarletToolkit.Observables;

namespace Maple
{
    public class FolderBrowserContentDialogViewModel : ObservableObject
    {
        private readonly FileSystemBrowserOptions _options;
        private FileSystemViewModel _fileSystemViewModel;

        public FileSystemViewModel FileSystemViewModel
        {
            get { return _fileSystemViewModel; }
            private set { SetValue(ref _fileSystemViewModel, value); }
        }

        public FolderBrowserContentDialogViewModel(FileSystemViewModel fileSystemViewModel, FileSystemBrowserOptions options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
            FileSystemViewModel = fileSystemViewModel ?? throw new ArgumentNullException(nameof(fileSystemViewModel));
        }
    }
}
