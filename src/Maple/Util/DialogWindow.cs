using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using MvvmScarletToolkit;
using MvvmScarletToolkit.Abstractions;

namespace Maple
{
    public class DialogWindow : IoCWindow
    {
        public ICommand AcceptCommand
        {
            get { return (ICommand)GetValue(AcceptCommandProperty); }
            set { SetValue(AcceptCommandProperty, value); }
        }

        public static readonly DependencyProperty AcceptCommandProperty = DependencyProperty.Register(
            nameof(AcceptCommand),
            typeof(ICommand),
            typeof(DialogWindow),
            new PropertyMetadata(default(ICommand)));

        private readonly IScarletCommandBuilder _commandBuilder;

        public DialogWindow()
        {
        }

        public DialogWindow(IScarletCommandBuilder commandBuilder, ILocalizationService localizationService, Window owner, CancellationToken abort)
            : base(commandBuilder, localizationService)
        {
            _commandBuilder = commandBuilder ?? throw new ArgumentNullException(nameof(commandBuilder));

            if (owner is null)
            {
                throw new ArgumentNullException(nameof(owner));
            }

            Owner = owner;
            AcceptCommand = commandBuilder.Create(Accept, CanAccept)
                .WithAsyncCancellation()
                .WithSingleExecution()
                .Build();

            abort.Register(Close);
        }

        protected virtual async Task Accept(CancellationToken token)
        {
            await _commandBuilder.Dispatcher.Invoke(() => DialogResult = true);
            await _commandBuilder.Dispatcher.Invoke(() => Close());
        }

        protected virtual bool CanAccept()
        {
            return true;
        }
    }
}
