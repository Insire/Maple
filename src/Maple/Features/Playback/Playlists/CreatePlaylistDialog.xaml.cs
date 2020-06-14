using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using MvvmScarletToolkit;
using MvvmScarletToolkit.Abstractions;

namespace Maple
{
    public partial class CreatePlaylistDialog : DialogWindow
    {
        private readonly CreatePlaylistViewModel _viewModel;

        public ICommand SelectThumbnailCommand
        {
            get { return (ICommand)GetValue(SelectThumbnailCommandProperty); }
            set { SetValue(SelectThumbnailCommandProperty, value); }
        }

        public static readonly DependencyProperty SelectThumbnailCommandProperty = DependencyProperty.Register(
            nameof(SelectThumbnailCommand),
            typeof(ICommand),
            typeof(CreatePlaylistDialog),
            new PropertyMetadata(default(ICommand)));

        public CreatePlaylistDialog()
        {
            InitializeComponent();
        }

        public CreatePlaylistDialog(IScarletCommandBuilder commandBuilder, ILocalizationService localizationService, CreatePlaylistViewModel viewModel, Window owner, CancellationToken abort)
            : base(commandBuilder, localizationService, owner, abort)
        {
            _viewModel = viewModel ?? throw new ArgumentNullException(nameof(viewModel));

            InitializeComponent();

            DataContext = viewModel;

            SelectThumbnailCommand = commandBuilder.Create(SelectThumbnail, CanSelectThumbnail)
                .WithAsyncCancellation()
                .WithSingleExecution()
                .Build();
        }

        public CreatePlaylistDialog(CreatePlaylistViewModel dataContext, IoCWindow owner, CancellationToken abort)
            : this(owner.CommandBuilder, owner.LocalizationService, dataContext, owner, abort)
        {
        }

        protected override bool CanAccept()
        {
            return _viewModel.CanSave() && base.CanAccept();
        }

        private Task SelectThumbnail(CancellationToken token)
        {
            var dlg = new FileSystemBrowserDialog(this, token, _viewModel.FileSystemViewModel);
            dlg.ShowDialog();

            return Task.CompletedTask;
        }

        private bool CanSelectThumbnail()
        {
            return true;
        }
    }
}
