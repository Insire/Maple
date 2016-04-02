using System.Collections.Generic;
using System.Linq;
using GalaSoft.MvvmLight;
using InsireBot.MediaPlayer;

namespace InsireBot.ViewModel
{
    public class NewPlaylistViewModel : ViewModelBase
    {
        private RangeObservableCollection<Playlist> _items;
        public RangeObservableCollection<Playlist> Items
        {
            get { return _items; }
            set
            {
                _items = value;
                RaisePropertyChanged(nameof(Items));
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

        public bool AreAllItemsSelected
        {
            get { return Items.All(p => p.IsSelected); }
            set
            {
                Items.ToList().ForEach(p => p.IsSelected = value);
                RaisePropertyChanged(nameof(AreAllItemsSelected));
            }
        }

        public IEnumerable<Playlist> SelectedItems
        {
            get { return Items.Where(p => p.IsSelected); }
        }

        public Playlist this[int index]
        {
            get
            {
                return Items[index];
            }
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
