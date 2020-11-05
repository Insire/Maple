using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Maple.Domain;
using MvvmScarletToolkit;

namespace Maple
{
    public partial class RepeatModeControl : UserControl
    {
        public ICommand ToggleCommand
        {
            get { return (ICommand)GetValue(ToggleCommandProperty); }
            set { SetValue(ToggleCommandProperty, value); }
        }

        public static readonly DependencyProperty ToggleCommandProperty = DependencyProperty.Register(
            nameof(ToggleCommand),
            typeof(ICommand),
            typeof(RepeatModeControl),
            new PropertyMetadata(default(ICommand)));

        private Shell _shell;

        public RepeatModeControl()
        {
            InitializeComponent();
        }

        private void RepeatModeControl_Loaded(object sender, RoutedEventArgs e)
        {
            _shell = this.FindParent<Shell>();

            ToggleCommand = _shell.CommandBuilder.Create(Toggle, CanToggle)
                .WithSingleExecution()
                .Build();
        }

        private Task Toggle(CancellationToken token)
        {
            if (!(DataContext is RepeatMode mode))
            {
                return Task.CompletedTask;
            }

            return mode switch
            {
                RepeatMode.None => _shell.Invoke(() => DataContext = RepeatMode.Single, token),
                RepeatMode.Single => _shell.Invoke(() => DataContext = RepeatMode.All, token),
                RepeatMode.All => _shell.Invoke(() => DataContext = RepeatMode.None, token),
                _ => Task.CompletedTask,
            };
        }

        private bool CanToggle()
        {
            return DataContext is RepeatMode;
        }
    }
}
