using DryIoc;
using System.Windows.Input;

namespace InsireBot
{
    public partial class Shell : IoCWindow
    {
        public Shell(IContainer container):base(container)
        {
            InitializeComponent();
        }

        private void UIElement_OnPreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            MenuToggleButton.IsChecked = false;
        }
    }
}
