using System.Windows.Input;

namespace InsireBot
{
    public partial class Shell : IoCWindow
    {
        public Shell(ITranslationManager manager):base(manager)
        {
            InitializeComponent();
        }

        private void UIElement_OnPreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            MenuToggleButton.IsChecked = false;
        }
    }
}
