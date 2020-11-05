using System;
using System.Threading;
using System.Windows;
using MvvmScarletToolkit;
using MvvmScarletToolkit.Abstractions;

namespace Maple
{
    public partial class CreateMediaPlayerDialog : DialogWindow
    {
        public CreateMediaPlayerDialog()
        {
        }

        public CreateMediaPlayerDialog(IScarletCommandBuilder commandBuilder, ILocalizationService localizationService, CreateMediaPlayerViewModel dataContext, Window owner, CancellationToken abort)
            : base(commandBuilder, localizationService, owner, abort)
        {
            if (dataContext is null)
            {
                throw new ArgumentNullException(nameof(dataContext));
            }

            InitializeComponent();

            DataContext = dataContext;
        }

        public CreateMediaPlayerDialog(CreateMediaPlayerViewModel dataContext, IoCWindow owner, CancellationToken abort)
            : this(owner.CommandBuilder, owner.LocalizationService, dataContext, owner, abort)
        {
        }
    }
}
