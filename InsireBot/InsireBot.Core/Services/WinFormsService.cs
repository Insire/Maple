using System;
using System.Windows.Forms;

namespace InsireBotCore
{
    public static class WinFormsService
    {
        public static OpenFileDialog GetOpenFileDialog()
        {
            var dialog = new OpenFileDialog
            {
                CheckFileExists = true,
                CheckPathExists = true,
                InitialDirectory = Environment.CurrentDirectory,
                Multiselect = false,
                Title = "Select a file...",
                ValidateNames = true,
                DereferenceLinks = true,
                AddExtension = false,
            };

            return dialog;
        }

        public static FolderBrowserDialog GetFolderBrowserDialog(Environment.SpecialFolder rootFolder = Environment.SpecialFolder.MyMusic)
        {
            var dialog = new FolderBrowserDialog
            {
                Description = "Select a folder...",
                RootFolder = rootFolder,
                ShowNewFolderButton = true,
            };

            return dialog;
        }
    }
}
