using System;
using System.ComponentModel;
using System.Windows;

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

        private FrameworkElement _content;
        public FrameworkElement Content
        {
            get { return _content; }
            set
            {
                this.MutateVerbose(ref _content, value, RaisePropertyChanged());
            }
        }

        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                this.MutateVerbose(ref _isSelected, value, RaisePropertyChanged());

                if (_isSelected)
                {
                    var datacontext = GetDataContext?.Invoke();
                    if (datacontext != null)
                        Content.DataContext = datacontext;
                }
            }
        }

        public Func<object> GetDataContext { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private Action<PropertyChangedEventArgs> RaisePropertyChanged()
        {
            return args => PropertyChanged?.Invoke(this, args);
        }
    }
}
