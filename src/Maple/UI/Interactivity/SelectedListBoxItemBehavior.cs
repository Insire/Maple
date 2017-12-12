using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using Maple.Core;
using Maple.Interfaces;

namespace Maple
{
    public class SelectedListBoxItemBehavior : Behavior<ListBox>
    {
        public IRangeObservableCollection<object> SelectedItems
        {
            get { return (IRangeObservableCollection<object>)GetValue(SelectedItemsProperty); }
            set { SetValue(SelectedItemsProperty, value); }
        }

        public static readonly DependencyProperty SelectedItemsProperty = DependencyProperty.Register(
            nameof(SelectedItems),
            typeof(IRangeObservableCollection<object>),
            typeof(SelectedListBoxItemBehavior),
            new UIPropertyMetadata(new RangeObservableCollection<object>(), OnSelectedItemsChanged)); // providing a new RangeObservableCollection is actually a bug here, as its the same collection across all instances of this behavior

        private static void OnSelectedItemsChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var item = e.NewValue as ListBoxItem;

            item?.SetCurrentValue(ListBoxItem.IsSelectedProperty, true);
        }

        protected override void OnAttached()
        {
            base.OnAttached();

            if (AssociatedObject != null)
                AssociatedObject.SelectionChanged += SelectionChanged;
        }

        private void SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedItems?.RemoveRange(e.RemovedItems.Cast<object>());
            SelectedItems?.AddRange(e.AddedItems.Cast<object>());
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            if (AssociatedObject != null)
                AssociatedObject.SelectionChanged -= SelectionChanged;
        }
    }
}
