using System.Windows;
using System.Windows.Controls;
using MvvmScarletToolkit.FileSystemBrowser;

namespace Maple
{
    public class FileSystemBrowserDetailTemplateSelector : DataTemplateSelector
    {
        public DataTemplate DriveTemplate { get; set; }
        public DataTemplate FolderTemplate { get; set; }
        public DataTemplate FileTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            switch (item)
            {
                case ScarletFile _:
                    return FileTemplate;

                case ScarletDirectory _:
                    return FolderTemplate;

                case ScarletDrive _:
                    return DriveTemplate;
            }

            return base.SelectTemplate(item, container);
        }
    }
}
