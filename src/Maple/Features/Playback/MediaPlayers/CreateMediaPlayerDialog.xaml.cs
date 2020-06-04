using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using MvvmScarletToolkit;

namespace Maple
{
    public partial class CreateMediaPlayerDialog : Window
    {
        public ICommand AcceptCommand
        {
            get { return (ICommand)GetValue(AcceptCommandProperty); }
            set { SetValue(AcceptCommandProperty, value); }
        }

        public static readonly DependencyProperty AcceptCommandProperty = DependencyProperty.Register(
            nameof(AcceptCommand),
            typeof(ICommand),
            typeof(CreateMediaPlayerDialog),
            new PropertyMetadata(default(ICommand)));

        private readonly IScarletCommandBuilder _commandBuilder;

        public CreateMediaPlayerDialog(IScarletCommandBuilder commandBuilder, CreateMediaPlayerViewModel dataContext, Window owner, CancellationToken abort)
        {
            _commandBuilder = commandBuilder ?? throw new ArgumentNullException(nameof(commandBuilder));

            if (dataContext is null)
            {
                throw new ArgumentNullException(nameof(dataContext));
            }

            if (owner is null)
            {
                throw new ArgumentNullException(nameof(owner));
            }

            InitializeComponent();

            AcceptCommand = commandBuilder.Create(Accept, CanAccept)
                .WithAsyncCancellation()
                .WithSingleExecution()
                .Build();

            Owner = owner;
            DataContext = dataContext;

            abort.Register(Close);
        }

        private async Task Accept(CancellationToken token)
        {
            await _commandBuilder.Dispatcher.Invoke(() => DialogResult = true);
            await _commandBuilder.Dispatcher.Invoke(() => Close());
        }

        private bool CanAccept()
        {
            return true;
        }
    }
}
