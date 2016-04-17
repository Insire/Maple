using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using InsireBotCore;

namespace InsireBot.ViewModel
{
    /// <summary>
    ///  This ViewModel will take care of adding Items from the parsed data of NewMediaItemViewModel and NewPlaylistViewModel to the BusinessModels
    /// </summary>
    public class NewMediaItemsDialogViewModel : ViewModelBase
    {
        private NewPlaylistViewModel _newPlaylistViewModel;
        public NewPlaylistViewModel NewPlaylistViewModel
        {
            get { return _newPlaylistViewModel; }
            set
            {
                _newPlaylistViewModel = value;
                RaisePropertyChanged(nameof(NewPlaylistViewModel));
            }
        }

        private NewMediaItemViewModel _newMediaItemViewModel;
        public NewMediaItemViewModel NewMediaItemViewModel
        {
            get { return _newMediaItemViewModel; }
            set
            {
                _newMediaItemViewModel = value;
                RaisePropertyChanged(nameof(NewMediaItemViewModel));
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
                RaisePropertyChanged(nameof(IsBusy));
                RaisePropertyChanged(nameof(CanAdd));
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

        public ICommand ParseCommand { get; private set; }
        public ICommand AddCommand { get; private set; }
        public ICommand SelectCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }
        public ICommand ClearCommand { get; private set; }
        public ICommand RemoveCommand { get; private set; }

        public NewMediaItemsDialogViewModel()
        {
            Mode = MediaItemParseMode.MediaItem;
            NewMediaItemViewModel = new NewMediaItemViewModel();
            NewPlaylistViewModel = new NewPlaylistViewModel();

            IntializeCommands();
        }

        private void IntializeCommands()
        {
            SelectCommand = new RelayCommand(() =>
            {
                var dialog = WinFormsService.GetOpenFileDialog();

                var result = dialog.ShowDialog();

                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    var fileName = dialog.FileName;
                    var mediaItem = new MediaItem(Path.GetFileNameWithoutExtension(fileName), new Uri(fileName));
                    NewMediaItemViewModel.Items.Add(mediaItem);
                }
            });

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
                    case MediaItemParseMode.MediaItem:
                        var items = NewMediaItemViewModel.Items.Where(p => p.IsSelected && !p.IsRestricted);
                        if (items.Any())
                        {
                            foreach (var item in NewMediaItemViewModel.Items)
                                Messenger.Default.Send(item);
                        }
                        else
                            foreach (var item in NewMediaItemViewModel.Items)
                                Messenger.Default.Send(item);

                        break;
                    case MediaItemParseMode.Playlist:
                        foreach (var item in NewPlaylistViewModel.Items)
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

            RemoveCommand = new RelayCommand(() =>
            {
                switch (Mode)
                {
                    case MediaItemParseMode.MediaItem:
                        NewMediaItemViewModel.SelectedItems.ToList().ForEach(p => NewMediaItemViewModel.Items.Remove(p));
                        break;

                    case MediaItemParseMode.Playlist:
                        NewPlaylistViewModel.SelectedItems.ToList().ForEach(p => NewPlaylistViewModel.Items.Remove(p));
                        break;

                    default:
                        throw new NotImplementedException();
                }
            }, CanRemove);

            ClearCommand = new RelayCommand(() =>
            {
                switch (Mode)
                {
                    case MediaItemParseMode.MediaItem:
                        NewMediaItemViewModel.Items.Clear();
                        break;

                    case MediaItemParseMode.Playlist:
                        NewPlaylistViewModel.Items.Clear();
                        break;

                    default:
                        throw new NotImplementedException();
                }
            }, CanClear);
        }

        private bool CanAdd()
        {
            switch (Mode)
            {
                case MediaItemParseMode.MediaItem:
                    return NewMediaItemViewModel.CanClear() && !IsBusy;

                case MediaItemParseMode.Playlist:
                    return NewPlaylistViewModel.CanClear() && !IsBusy;

                default:
                    throw new NotImplementedException();
            }
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
            switch (Mode)
            {
                case MediaItemParseMode.MediaItem:
                    return NewMediaItemViewModel.Items?.Count > 0;

                case MediaItemParseMode.Playlist:
                    return NewPlaylistViewModel.Items?.Count > 0;

                default:
                    throw new NotImplementedException();
            }
        }

        private bool AreItemsSelected()
        {
            switch (Mode)
            {
                case MediaItemParseMode.MediaItem:
                    return NewMediaItemViewModel.Items.Any(p => p.IsSelected);

                case MediaItemParseMode.Playlist:
                    return NewPlaylistViewModel.Items.Any(p => p.IsSelected);

                default:
                    throw new NotImplementedException();
            }
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
                            NewMediaItemViewModel.Items.AddRange(video);

                            continue;
                        }

                        if (key == "list")
                        {
                            IsBusy = true;

                            var playlists = await youtube.GetPlaylist(id);
                            NewPlaylistViewModel.Items.AddRange(playlists);

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
            switch (Mode)
            {
                case MediaItemParseMode.MediaItem:
                    if (NewMediaItemViewModel.AreAllItemsSelected)
                        NewMediaItemViewModel.Items.All(p => p.IsSelected = true);
                    else
                        NewMediaItemViewModel.Items.All(p => p.IsSelected = false);
                    break;

                case MediaItemParseMode.Playlist:
                    if (NewPlaylistViewModel.AreAllItemsSelected)
                        NewPlaylistViewModel.Items.All(p => p.IsSelected = true);
                    else
                        NewPlaylistViewModel.Items.All(p => p.IsSelected = false);
                    break;
            }
        }
    }

    public enum MediaItemParseMode
    {
        MediaItem,
        Playlist
    }
}
