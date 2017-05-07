using Maple.Core;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace Maple
{
    public class SelectedListBoxItemBehavior : Behavior<ListBox>
    {
        private static Dictionary<object, ListBox> _cache;
        private static ListBox _currentSource;

        static SelectedListBoxItemBehavior()
        {
            _cache = new Dictionary<object, ListBox>();
        }

        public RangeObservableCollection<object> SelectedItems
        {
            get { return (RangeObservableCollection<object>)GetValue(SelectedItemsProperty); }
            set { SetValue(SelectedItemsProperty, value); }
        }

        public static readonly DependencyProperty SelectedItemsProperty = DependencyProperty.Register(
                                                                                nameof(SelectedItems),
                                                                                typeof(RangeObservableCollection<object>),
                                                                                typeof(SelectedListBoxItemBehavior),
                                                                                new UIPropertyMetadata(new RangeObservableCollection<object>(), OnSelectionChanged));

        private static void OnSelectionChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var item = e.NewValue as ListBoxItem;

            item?.SetValue(ListBoxItem.IsSelectedProperty, true);
        }

        protected override void OnAttached()
        {
            base.OnAttached();

            if (AssociatedObject != null)
                AssociatedObject.SelectionChanged += SelectionChanged;
        }

        private void SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!ReferenceEquals(_currentSource, e.Source))
            {
                SelectedItems.Clear();
                _currentSource = e.Source as ListBox;
            }

            SelectedItems?.RemoveRange(e.RemovedItems);
            SelectedItems?.AddRange(e.AddedItems);
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            if (AssociatedObject != null)
                AssociatedObject.SelectionChanged -= SelectionChanged;
        }
    }
}
