using System;
using System.ComponentModel;

namespace InsireBotCore
{
    /// <summary>
    /// this class represents a wrapper for 2 ui controls
    /// fancy stuff for displaying a clickable list with more clickable stuff inside each item
    /// </summary>
    public class DrawerItem : INotifyPropertyChanged, IIsSelected
    {
        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                this.MutateVerbose(ref _name, value, RaisePropertyChanged());
            }
        }

        private object _content;
        public object Content
        {
            get { return _content; }
            set
            {
                this.MutateVerbose(ref _content, value, RaisePropertyChanged());
            }
        }

        private DrawerItem _detail;
        public DrawerItem Detail
        {
            get { return _detail; }
            set
            {
                this.MutateVerbose(ref _detail, value, RaisePropertyChanged());
            }
        }

        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                this.MutateVerbose(ref _isSelected, value, RaisePropertyChanged());
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private Action<PropertyChangedEventArgs> RaisePropertyChanged()
        {
            return args => PropertyChanged?.Invoke(this, args);
        }
    }
}
