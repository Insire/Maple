using System;
using System.Windows.Input;

namespace Maple.Core
{
    public abstract class MapleFileSystemBase : ObservableObject, IFileSystemInfo
    {
        protected readonly BusyStack _busyStack;

        public static bool NoFilesFilter(object obj)
        {
            if (obj is IFileSystemFile)
                return false;

            return SearchFilter(obj);
        }

        public static bool SearchFilter(object obj)
        {
            var info = obj as IFileSystemInfo;

            if (info == null)
                return false;

            if (info.IsHidden || info.IsBusy)
                return false;

            if (string.IsNullOrWhiteSpace(info.Filter))
                return true;

            return info.Name.ToLowerInvariant().Contains(info.Filter.ToLowerInvariant());
        }

        public ICommand LoadCommand { get; protected set; }
        public ICommand RefreshCommand { get; protected set; }
        public ICommand DeleteCommand { get; protected set; }

        private IFileSystemDirectory _parent;
        public IFileSystemDirectory Parent
        {
            get { return _parent; }
            protected set { SetValue(ref _parent, value); }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            protected set { SetValue(ref _name, value); }
        }

        private string _fullName;
        public string FullName
        {
            get { return _fullName; }
            protected set { SetValue(ref _fullName, value); }
        }

        private string _filter;
        public string Filter
        {
            get { return _filter; }
            set { SetValue(ref _filter, value); }
        }

        private bool _exists;
        public bool Exists
        {
            get { return _exists; }
            protected set { SetValue(ref _exists, value); }
        }

        private bool _isBusy;
        public bool IsBusy
        {
            get { return _isBusy; }
            private set { SetValue(ref _isBusy, value); }
        }

        private bool _isLoaded;
        public bool IsLoaded
        {
            get { return _isLoaded; }
            protected set { SetValue(ref _isLoaded, value); }
        }

        private bool _isExpanded;
        public bool IsExpanded
        {
            get { return _isExpanded; }
            set { SetValue(ref _isExpanded, value, OnChanged: OnExpandedChanged); }
        }

        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set { SetValue(ref _isSelected, value); }
        }

        private bool _isHidden;
        public bool IsHidden
        {
            get { return _isHidden; }
            protected set { SetValue(ref _isHidden, value); }
        }

        private bool _isContainer;
        public bool IsContainer
        {
            get { return _isContainer; }
            protected set { SetValue(ref _isContainer, value); }
        }

        private bool _hasContainers;
        public bool HasContainers
        {
            get { return _hasContainers; }
            protected set { SetValue(ref _hasContainers, value); }
        }

        private IDepth _depth;
        public IDepth Depth
        {
            get { return _depth; }
            protected set { SetValue(ref _depth, value); }
        }

        private MapleFileSystemBase()
        {
            _busyStack = new BusyStack();
            _busyStack.OnChanged += (hasItems) => IsBusy = hasItems;

            LoadCommand = new RelayCommand(Load, CanLoad);
            RefreshCommand = new RelayCommand(Refresh, CanRefresh);
            DeleteCommand = new RelayCommand(Delete, CanDelete);

            Exists = true;
            IsHidden = false;
            IsContainer = false;
            HasContainers = false;
        }

        protected MapleFileSystemBase(string name, string fullName, IDepth depth, IFileSystemDirectory parent) : this()
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException($"{nameof(Name)} can't be empty.", nameof(Name));

            if (string.IsNullOrEmpty(fullName))
                throw new ArgumentException($"{nameof(FullName)} can't be empty.", nameof(FullName));

            if (depth == null)
                throw new ArgumentException($"{nameof(Depth)} can't be empty.", nameof(Depth));

            if (!(this is IFileSystemDrive) && parent == null)
                throw new ArgumentException($"{nameof(Parent)} can't be empty.", nameof(Parent));

            using (_busyStack.GetToken())
            {
                Depth = depth;
                Depth.Current++;

                Name = name;
                FullName = fullName;
                Parent = parent;
            }
        }

        public void Load()
        {
            if (IsLoaded)
                return;

            Refresh();
        }

        public abstract void Refresh();
        public abstract void LoadMetaData();
        public abstract void OnFilterChanged(string filter);
        public abstract void Delete();
        public abstract bool CanDelete();

        protected bool CanLoad()
        {
            return !IsBusy && !IsLoaded;
        }

        protected bool CanRefresh()
        {
            return !IsBusy;
        }

        private void OnExpandedChanged()
        {
            if (!IsBusy && !IsLoaded)
            {
                Load();
                IsLoaded = true;
            }
        }
    }
}
