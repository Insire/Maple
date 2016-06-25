using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using InsireBot.ViewModel;
using InsireBotCore;

namespace InsireBot.Utils
{
    /// <summary>
    /// generates MediaItems and  Playlists from input
    /// </summary>
    public class DataParsingService : ObservableObject
    {
        private PlaylistsStore _playlists;
        public PlaylistsStore PlaylistsCache
        {
            get { return _playlists; }
            set
            {
                _playlists = value;
                RaisePropertyChanged(nameof(PlaylistsCache));
            }
        }

        private MediaItemsStore _mediaItems;
        public MediaItemsStore MediaItemsCache
        {
            get { return _mediaItems; }
            set
            {
                _mediaItems = value;
                RaisePropertyChanged(nameof(MediaItemsCache));
            }
        }

        public void Parse(FileInfo file)
        {
            //assume its a MediaFile aka *.mp3
            var mediaItem = new MediaItem(Path.GetFileNameWithoutExtension(file.FullName), new Uri(file.FullName));
            Messenger.Default.Send(mediaItem);
        }

        public void Parse(string data)
        {
            var url = new Uri(data.Trim());
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

                    if (key == "v")
                    {
                        var videos = await youtube.GetVideo(id);
                        MediaItemsCache.Items.AddRange(videos);

                        foreach (var video in videos)
                            Messenger.Default.Send(video);

                        continue;
                    }

                    if (key == "list")
                    {
                        var playlists = await youtube.GetPlaylist(id);
                        PlaylistsCache.AddRange(playlists);

                        continue;
                    }

                }

                rx = rx.NextMatch();
            }
        }
    }
}
