using System.Windows;
using System.Windows.Controls;
using MvvmScarletToolkit.FileSystemBrowser;

namespace Maple
{
    public sealed class FileSystemBrowserDetailTemplateSelector : DataTemplateSelector
    {
        public DataTemplate DriveTemplate { get; set; }
        public DataTemplate FolderTemplate { get; set; }
        public DataTemplate FileTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            return item switch
            {
                ScarletFile _ => FileTemplate,
                ScarletDirectory _ => FolderTemplate,
                ScarletDrive _ => DriveTemplate,

                _ => base.SelectTemplate(item, container),
            };
        }
    }
}
