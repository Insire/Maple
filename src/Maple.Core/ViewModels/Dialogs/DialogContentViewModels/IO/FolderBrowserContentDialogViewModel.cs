using System;

using Maple.Localization.Properties;

namespace Maple.Core
{
    public class FolderBrowserContentDialogViewModel : ObservableObject
    {
        private FileSystemViewModel _fileSystemViewModel;
        public FileSystemViewModel FileSystemViewModel
        {
            get { return _fileSystemViewModel; }
            private set { SetValue(ref _fileSystemViewModel, value); }
        }

        public FolderBrowserContentDialogViewModel(FileSystemViewModel fileSystemViewModel)
        {
            FileSystemViewModel = fileSystemViewModel ?? throw new ArgumentNullException(nameof(fileSystemViewModel), $"{nameof(fileSystemViewModel)} {Resources.IsRequired}");
        }
    }
}
