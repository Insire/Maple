using System.Collections.Generic;
using System.Linq;

using GalaSoft.MvvmLight;

using InsireBotCore;

namespace InsireBot.ViewModel
{
    /// <summary>
    ///  DataStore ViewModel, when creating new playlist entries aka videos from parsed data
    /// </summary>
    public class NewMediaItemViewModel : ViewModelBase
    {
        private RangeObservableCollection<IMediaItem> _items;
        public RangeObservableCollection<IMediaItem> Items
        {
            get { return _items; }
            set
            {
                _items = value;
                RaisePropertyChanged(nameof(Items));
            }
        }

        public bool AreAllItemsSelected
        {
            get { return Items.All(p => p.IsSelected); }
            set
            {
                Items.ToList().ForEach(p => p.IsSelected = value);
                RaisePropertyChanged(nameof(AreAllItemsSelected));
            }
        }

        private bool _isBusy;
        public bool IsBusy
        {
            get { return _isBusy; }
            private set
            {
                _isBusy = value;
                RaisePropertyChanged(nameof(Items));
                RaisePropertyChanged(nameof(IsBusy));
                RaisePropertyChanged(nameof(CanAdd));
            }
        }

        public IEnumerable<IMediaItem> SelectedItems
        {
            get { return Items.Where(p => p.IsSelected); }
        }

        public IMediaItem this[int index]
        {
            get
            {
                return Items[index];
            }
        }

        public NewMediaItemViewModel()
        {
            Items = new RangeObservableCollection<IMediaItem>();
        }

        /// <summary>
        /// Can an Item be added to the Items Collection
        /// </summary>
        /// <returns></returns>
        public bool CanAdd()
        {
            return Items != null;
        }

        /// <summary>
        /// Can an Item be removed from the Items Collection. requires selected item
        /// </summary>
        /// <returns></returns>
        public bool CanRemove()
        {
            return AreItemsSelected();
        }

        public bool CanClear()
        {
            return CanAdd() && Items.Any();
        }

        public bool AreItemsSelected()
        {
            return Items.Any(p => p.IsSelected);
        }
    }
}
