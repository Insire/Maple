using System;
using System.Windows.Forms;

namespace InsireBotCore
{
    /// <summary>
    ///
    /// </summary>
    public static class WinFormsService
    {
        /// <summary>
        /// Gets the open file dialog.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Gets the folder browser dialog.
        /// </summary>
        /// <param name="rootFolder">The root folder.</param>
        /// <returns></returns>
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
