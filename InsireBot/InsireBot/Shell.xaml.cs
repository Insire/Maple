using Maple.Core;
using System;
using System.Windows.Input;

namespace Maple
{
    public partial class Shell : IoCWindow
    {
        public Shell(ITranslationService manager, IUIColorsViewModel vm, ShellViewModel datacontext) : base(manager, vm)
        {
            DataContext = datacontext ?? throw new ArgumentNullException(nameof(datacontext));

            InitializeComponent();
        }

        private void UIElement_OnPreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            MenuToggleButton.IsChecked = false;
        }
    }
}
