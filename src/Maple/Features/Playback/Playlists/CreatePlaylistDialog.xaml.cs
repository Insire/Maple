using System;
using System.Threading;
using System.Windows;
using MvvmScarletToolkit;
using MvvmScarletToolkit.Abstractions;

namespace Maple
{
    public partial class CreatePlaylistDialog : DialogWindow
    {
        public CreatePlaylistDialog()
        {
            InitializeComponent();
        }

        public CreatePlaylistDialog(IScarletCommandBuilder commandBuilder, ILocalizationService localizationService, CreatePlaylistViewModel dataContext, Window owner, CancellationToken abort)
            : base(commandBuilder, localizationService, owner, abort)
        {
            if (dataContext is null)
            {
                throw new ArgumentNullException(nameof(dataContext));
            }

            InitializeComponent();

            DataContext = dataContext;
        }

        public CreatePlaylistDialog(CreatePlaylistViewModel dataContext, IoCWindow owner, CancellationToken abort)
            : this(owner.CommandBuilder, owner.LocalizationService, dataContext, owner, abort)
        {
        }
    }
}
