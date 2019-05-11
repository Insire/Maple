using System.Collections;
using System.IO;
using System.Linq;
using System.Windows.Input;

using Maple.Domain;
using MvvmScarletToolkit;
using MvvmScarletToolkit.Commands;

namespace Maple.Core
{
    public class FileSystemViewModel : ViewModel
    {
        private ICommand _selectCommand;
        /// <summary>
        /// command to update the selected folder or drive in the master view
        /// </summary>
        public ICommand SelectCommand
        {
            get { return _selectCommand; }
            private set { SetValue(ref _selectCommand, value); }
        }

        private IRangeObservableCollection<IFileSystemInfo> _selectedItems;
        /// <summary>
        /// contains either folders or files ready to be used somewhere else
        /// </summary>
        public IRangeObservableCollection<IFileSystemInfo> SelectedItems
        {
            get { return _selectedItems; }
            private set { SetValue(ref _selectedItems, value); }
        }

        private IList _selectedItemsList;
        /// <summary>
        /// bound to contain selected folders and files from the detail view
        /// </summary>
        public IList SelectedItemsList
        {
            get { return _selectedItemsList; }
            set { SetValue(ref _selectedItemsList, value, OnChanged: OnSelectedItemsListChanged); }
        }

        private IRangeObservableCollection<MapleDrive> _drives;
        /// <summary>
        /// the currently available drives of the filesystem
        /// </summary>
        public IRangeObservableCollection<MapleDrive> Drives
        {
            get { return _drives; }
            private set { SetValue(ref _drives, value); }
        }

        private MapleFileSystemContainerBase _selectedItem;
        /// <summary>
        /// the selected item in the master view
        /// </summary>
        public MapleFileSystemContainerBase SelectedItem
        {
            get { return _selectedItem; }
            set { SetValue(ref _selectedItem, value, OnChanged: OnSelectedItemChanged); }
        }

        private string _filter;
        /// <summary>
        /// text to search for in the items in the detailview (source is the master view)
        /// </summary>
        public string Filter
        {
            get { return _filter; }
            set { SetValue(ref _filter, value, OnChanged: () => SelectedItem.OnFilterChanged(Filter)); }
        }

        private bool _displayListView;
        /// <summary>
        /// flag to switch between different display styles for the detail view
        /// </summary>
        public bool DisplayListView
        {
            get { return _displayListView; }
            set { SetValue(ref _displayListView, value); }
        }

        public FileSystemViewModel(IMessenger messenger, ILoggingService loggingService)
            : base(messenger)
        {
            using (BusyStack.GetToken())
            {
                DisplayListView = false;

                Drives = new RangeObservableCollection<MapleDrive>();
                SelectedItems = new RangeObservableCollection<IFileSystemInfo>();

                var drives = DriveInfo.GetDrives()
                                        .Where(p => p.IsReady && p.DriveType != DriveType.CDRom && p.DriveType != DriveType.Unknown)
                                        .Select(p => new MapleDrive(p, new FileSystemDepth(0), Messenger, loggingService))
                                        .ToList();
                Drives.AddRange(drives);

                SelectCommand = new RelayCommand<object>(SetSelectedItem, CanSetSelectedItem);
            }
        }

        private void OnSelectedItemsListChanged()
        {
            SelectedItems.Clear();
            SelectedItems.AddRange(SelectedItemsList.Cast<IFileSystemInfo>());
        }

        private void OnSelectedItemChanged()
        {
            if (SelectedItem == null)
                return;

            SelectedItem.Load();
            SelectedItem.LoadMetaData();

            Messenger.Publish(new FileSystemInfoChangedMessage(this, SelectedItem));
        }

        public void SetSelectedItem(object item)
        {
            var value = item as MapleFileSystemContainerBase;

            if (value == null)
                return;

            SelectedItem = value;
            SelectedItem.ExpandPath();
            SelectedItem.Parent.IsSelected = true;
        }

        private bool CanSetSelectedItem(object item)
        {
            if (item == null)
                return false;

            var value = item as MapleFileSystemContainerBase;

            return value != null && value != SelectedItem;
        }
    }
}
