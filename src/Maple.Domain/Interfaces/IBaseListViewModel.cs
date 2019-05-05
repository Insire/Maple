using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;

namespace Maple.Domain
{
    public interface IBaseListViewModel<TViewModel> where TViewModel : INotifyPropertyChanged
    {
        TViewModel this[int index] { get; }

        ICommand AddCommand { get; }
        ICommand ClearCommand { get; }
        int Count { get; }
        IReadOnlyCollection<TViewModel> Items { get; }
        ICommand RemoveCommand { get; }
        ICommand RemoveRangeCommand { get; }
        TViewModel SelectedItem { get; set; }

        void Add(TViewModel item);

        void AddRange(IEnumerable<TViewModel> items);

        bool CanClear();

        bool CanRemove(TViewModel item);

        bool CanRemoveRange(IEnumerable<TViewModel> items);

        bool CanRemoveRange(IList items);

        void Clear();

        void Remove(TViewModel item);

        void RemoveRange(IEnumerable<TViewModel> items);

        void RemoveRange(IList items);
    }
}
