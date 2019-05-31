using System;
using System.Windows.Input;
using MvvmScarletToolkit.Abstractions;

namespace Maple
{
    public partial class Shell : IoCWindow
    {
        private readonly ShellViewModel _datacontext;

        public Shell(ILocalizationService manager, IScarletMessenger messenger, ShellViewModel datacontext) : base(manager, messenger)
        {
            DataContext = datacontext ?? throw new ArgumentNullException(nameof(datacontext), $"{nameof(datacontext)} {Localization.Properties.Resources.IsRequired}");
            _datacontext = datacontext;

            InitializeComponent();
        }

        private void UIElement_OnPreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _datacontext.NavigationViewModel.IsExpanded = false;
        }
    }
}
