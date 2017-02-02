using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Maple
{
    public static class DataGridBehavior
    {
        public static readonly DependencyProperty DoubleClickProperty = DependencyProperty
            .RegisterAttached("DoubleClickCommand",
            typeof(ICommand),
            typeof(DataGridBehavior),
            new PropertyMetadata(new PropertyChangedCallback(AttachOrRemoveDataGridDoubleClickEvent)));

        public static ICommand GetDoubleClickCommand(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(DoubleClickProperty);
        }

        public static void SetDoubleClickCommand(DependencyObject obj, ICommand value)
        {
            obj.SetValue(DoubleClickProperty, value);
        }

        public static void AttachOrRemoveDataGridDoubleClickEvent(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var dataGrid = obj as DataGrid;
            if (dataGrid != null)
            {
                var cmd = (ICommand)args.NewValue;

                if (args.OldValue == null && args.NewValue != null)
                    dataGrid.MouseDoubleClick += ExecuteDataGridDoubleClick;

                else
                {
                    if (args.OldValue != null && args.NewValue == null)
                        dataGrid.MouseDoubleClick -= ExecuteDataGridDoubleClick;
                }
            }
        }

        private static void ExecuteDataGridDoubleClick(object sender, MouseButtonEventArgs args)
        {
            var obj = sender as DependencyObject;
            var cmd = (ICommand)obj.GetValue(DoubleClickProperty);
            if (cmd != null)
            {
                if (cmd.CanExecute(obj))
                    cmd.Execute(obj);
            }
        }
    }
}
