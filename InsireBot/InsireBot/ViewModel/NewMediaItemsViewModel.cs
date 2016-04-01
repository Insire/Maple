using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using InsireBot.MediaPlayer;
using System.Web;
using GalaSoft.MvvmLight.Messaging;

namespace InsireBot.ViewModel
{
    public class NewMediaItemsViewModel : ViewModelBase
    {
        private RangeObservableCollection<MediaItem> _mediaItems;
        public RangeObservableCollection<MediaItem> MediaItems
        {
            get { return _mediaItems; }
            set
            {
                _mediaItems = value;
                RaisePropertyChanged(nameof(MediaItems));
            }
        }

        private RangeObservableCollection<Playlist> _playlists;
        public RangeObservableCollection<Playlist> Playlists
        {
            get { return _playlists; }
            set
            {
                _playlists = value;
                RaisePropertyChanged(nameof(Playlists));
            }
        }

        private bool _areAllItemsSelected;
        public bool AreAllItemsSelected
        {
            get { return _areAllItemsSelected; }
            set
            {
                _areAllItemsSelected = value;
                RaisePropertyChanged(nameof(AreAllItemsSelected));
            }
        }

        private string _sourceText;
        public string SourceText
        {
            get { return _sourceText; }
            set
            {
                _sourceText = value;
                RaisePropertyChanged(nameof(SourceText));
                RaisePropertyChanged(nameof(CanParse));
                RaisePropertyChanged(nameof(CanAdd));
                RaisePropertyChanged(nameof(CanCancel));
            }
        }

        private MediaItemParseMode _mode;
        public MediaItemParseMode Mode
        {
            get { return _mode; }
            set
            {
                _mode = value;
                RaisePropertyChanged(nameof(Mode));
            }
        }

        private bool _isBusy;
        public bool IsBusy
        {
            get { return _isBusy; }
            private set
            {
                _isBusy = value;
                RaisePropertyChanged(nameof(MediaItems));
                RaisePropertyChanged(nameof(Playlists));
                RaisePropertyChanged(nameof(IsBusy));
                RaisePropertyChanged(nameof(CanParse));
                RaisePropertyChanged(nameof(CanAdd));
                RaisePropertyChanged(nameof(CanCancel));
            }
        }

        private Action _closeAction;
        public Action CloseAction
        {
            get { return _closeAction; }
            set
            {
                _closeAction = value;
                RaisePropertyChanged(nameof(CloseAction));
            }
        }

        public IEnumerable<T> SelectedItems
        {
            get { return Items.Where(p => p.IsSelected); }
        }

        public T this[int index]
        {
            get
            {
                return Items[index];
            }
        }

        public ICommand ParseCommand { get; }
        public ICommand AddCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand ClearCommand { get; }
        public ICommand RemoveCommand { get; }

        public NewMediaItemsViewModel()
        {
            MediaItems = new RangeObservableCollection<MediaItem>();
            Playlists = new RangeObservableCollection<Playlist>();
            Mode = MediaItemParseMode.Multiple;

            ParseCommand = new RelayCommand(() =>
            {
                var url = new Uri(SourceText.Trim());
                switch (url.DnsSafeHost)
                {
                    case "youtu.be":
                        url = new Uri(url.AbsoluteUri.Replace(@"youtu.be/", @"youtube.com/watch?v="));
                        Parse(url);
                        break;
                    case "www.youtube.com":
                        Parse(url);
                        break;
                }
            }, CanParse);

            AddCommand = new RelayCommand(() =>
            {
                switch (Mode)
                {
                    case MediaItemParseMode.Multiple:
                        var items = MediaItems.Where(p => p.IsSelected && !p.IsRestricted);
                        if (items.Any())
                        {
                            foreach (var item in MediaItems)
                                Messenger.Default.Send(item);
                        }
                        else
                            foreach (var item in MediaItems)
                                Messenger.Default.Send(item);

                        break;
                    case MediaItemParseMode.Playlist:
                        foreach (var item in Playlists)
                            Messenger.Default.Send(item);

                        break;

                    default: throw new NotImplementedException();
                }

                CloseAction();
            }, CanAdd);

            CancelCommand = new RelayCommand(() =>
            {
                CloseAction();
            }, CanCancel);

            RemoveCommand = new RelayCommand(() => SelectedItems.ToList().ForEach(p => Items.Remove(p)), CanRemove);
            ClearCommand = new RelayCommand(() => Items.Clear(), CanClear);
        }

        private bool CanAdd()
        {
            return MediaItems != null && MediaItems.Any();
        }

        private bool CanParse()
        {
            return !string.IsNullOrEmpty(SourceText);
        }

        private bool CanCancel()
        {
            return !IsBusy && CloseAction != null;
        }

        public bool CanRemove()
        {
            return AreItemsSelected();
        }

        private bool CanClear()
        {
            return Items?.Count > 0;
        }

        private bool AreItemsSelected()
        {
            return Items.Any(p => p.IsSelected);
        }

        private async void Parse(Uri url)
        {
            var regex = @"(?:http|https|)(?::\/\/|)(?:www.|m.|)(?:youtu\.be\/|youtube\.com(?:\/embed\/|\/v\/|\/watch\?v=|\|\/feeds\/api\/videos\/|\/user\S*[^\w\-\s]|\S*[^\w\-\s]))([\w\-]*)[a-z0-9;:@?&%=+\/\$_.-]*";
            var rx = Regex.Match(url.AbsoluteUri, regex);
            var youtube = new Youtube();

            while (rx.Success)
            {
                var keys = HttpUtility.ParseQueryString(url.Query).AllKeys;

                foreach (var key in keys)
                {
                    string id = HttpUtility.ParseQueryString(url.Query).Get(key);

                    try
                    {
                        if (key == "v")
                        {
                            IsBusy = true;

                            var video = await youtube.GetVideo(id);
                            MediaItems.AddRange(video);

                            continue;
                        }

                        if (key == "list")
                        {
                            IsBusy = true;

                            var playlists = await youtube.GetPlaylist(id);
                            Playlists.AddRange(playlists);

                            continue;
                        }
                    }
                    finally
                    {
                        IsBusy = false;

                        // WPF doesn't update command bound controls unless it has a reason to. Clicking on the GUI causes WPF to refresh so the update then works.
                        //You can manually cause a refresh of any command bound controls by calling
                        CommandManager.InvalidateRequerySuggested();
                    }
                }

                rx = rx.NextMatch();
            }
        }

        private void UpdateSelectedItems()
        {
            switch(Mode)
            {
                case MediaItemParseMode.Multiple:
                    if (AreAllItemsSelected)
                        MediaItems.All(p => p.IsSelected = true);
                    else
                        MediaItems.All(p => p.IsSelected = false);
                    break;

                case MediaItemParseMode.Playlist:
                    if (AreAllItemsSelected)
                        Playlists.All(p => p.IsSelected = true);
                    else
                        Playlists.All(p => p.IsSelected = false);
                    break;
            }
        }
    }

    public enum MediaItemParseMode
    {
        Multiple,
        Playlist
    }
}
