using System;
using System.Threading;
using System.Windows;
using MvvmScarletToolkit;
using MvvmScarletToolkit.Abstractions;
using MvvmScarletToolkit.Wpf.FileSystemBrowser;

namespace Maple
{
    public partial class FileSystemBrowserDialog : DialogWindow
    {
        private readonly FileSystemViewModel _fileSystemViewModel;

        public FileSystemBrowserDialog()
            : base()
        {
        }

        public FileSystemBrowserDialog(IScarletCommandBuilder commandBuilder, ILocalizationService localizationService, Window owner, CancellationToken abort, FileSystemViewModel fileSystemViewModel)
            : base(commandBuilder, localizationService, owner, abort)
        {
            _fileSystemViewModel = fileSystemViewModel ?? throw new ArgumentNullException(nameof(fileSystemViewModel));

            if (owner is null)
            {
                throw new ArgumentNullException(nameof(owner));
            }

            InitializeComponent();

            Owner = owner;
            DataContext = fileSystemViewModel;
        }

        public FileSystemBrowserDialog(DialogWindow owner, CancellationToken abort, FileSystemViewModel fileSystemViewModel)
            : this(owner.CommandBuilder, owner.LocalizationService, owner, abort, fileSystemViewModel)
        {
        }

        protected override bool CanAccept()
        {
            return _fileSystemViewModel.SelectedItem is ScarletFile;
        }
    }
}
