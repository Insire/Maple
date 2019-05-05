using System.Windows;
using System.Windows.Controls;

using Maple.Core;

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
                case MapleFile _:
                    return FileTemplate;

                case MapleDirectory _:
                    return FolderTemplate;

                case MapleDrive _:
                    return DriveTemplate;
            }

            return base.SelectTemplate(item, container);
        }
    }
}
