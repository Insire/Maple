using MvvmScarletToolkit;
using MvvmScarletToolkit.Abstractions;
using System;
using System.Threading;
using System.Windows;

namespace Maple
{
    public partial class MonstercatImportDialog
    {
        private readonly MonstercatImportViewModel _monstercatImportViewModel;

        public MonstercatImportDialog()
        {
            InitializeComponent();
        }

        public MonstercatImportDialog(IScarletCommandBuilder commandBuilder, ILocalizationService localizationService, Window owner, CancellationToken abort, MonstercatImportViewModel monstercatImportViewModel)
            : base(commandBuilder, localizationService, owner, abort)
        {
            _monstercatImportViewModel = monstercatImportViewModel ?? throw new ArgumentNullException(nameof(monstercatImportViewModel));

            if (owner is null)
            {
                throw new ArgumentNullException(nameof(owner));
            }

            InitializeComponent();

            Owner = owner;
            DataContext = monstercatImportViewModel;
        }

        public MonstercatImportDialog(DialogWindow owner, CancellationToken abort, MonstercatImportViewModel monstercatImportViewModel)
            : this(owner.CommandBuilder, owner.LocalizationService, owner, abort, monstercatImportViewModel)
        {
        }

        protected override bool CanAccept()
        {
            return true; // TODO ?
        }
    }
}
