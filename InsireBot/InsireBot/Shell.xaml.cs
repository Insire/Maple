using Maple.Core;
using System.Windows.Input;

namespace Maple
{
    public partial class Shell : IoCWindow
    {
        public Shell(ITranslationService manager, IUIColorsViewModel vm) : base(manager, vm)
        {
            InitializeComponent();
        }

        private void UIElement_OnPreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            MenuToggleButton.IsChecked = false;
        }
    }
}
