using Maple.Core;
using System;
using System.Windows.Input;

namespace Maple
{
    public partial class Shell : IoCWindow
    {
        private readonly ShellViewModel _datacontext;

        public Shell(ILocalizationService manager, IUIColorsViewModel vm, ShellViewModel datacontext) : base(manager, vm)
        {
            DataContext = datacontext ?? throw new ArgumentNullException(nameof(datacontext));
            _datacontext = datacontext;

            InitializeComponent();
        }

        private void UIElement_OnPreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _datacontext.Scenes.IsExpanded = false;
        }
    }
}
