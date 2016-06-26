using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
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

        public DataParsingServiceResult Parse(FileInfo file)
        {
            //assume its a MediaFile aka *.mp3 or something similar
            var mediaItem = new MediaItem(Path.GetFileNameWithoutExtension(file.FullName), new Uri(file.FullName));
            var result = new DataParsingServiceResult(mediaItem);
            Messenger.Default.Send(mediaItem);

            return result;
        }

        public async Task<DataParsingServiceResult> Parse(string data)
        {
            var url = new Uri(data.Trim());
            switch (url.DnsSafeHost)
            {
                case "youtu.be":
                    url = new Uri(url.AbsoluteUri.Replace(@"youtu.be/", @"youtube.com/watch?v="));
                    return await Parse(url);

                case "www.youtube.com":
                    return await Parse(url);

                default:
                    return new DataParsingServiceResult();
            }
        }

        private async Task<DataParsingServiceResult> Parse(Uri url)
        {
            DataParsingServiceResult result;
            var regex = @"(?:http|https|)(?::\/\/|)(?:www.|m.|)(?:youtu\.be\/|youtube\.com(?:\/embed\/|\/v\/|\/watch\?v=|\|\/feeds\/api\/videos\/|\/user\S*[^\w\-\s]|\S*[^\w\-\s]))([\w\-]*)[a-z0-9;:@?&%=+\/\$_.-]*";
            var match = Regex.Match(url.AbsoluteUri, regex);
            var youtubeService = new Youtube();

            while (match.Success)
            {
                var keys = HttpUtility.ParseQueryString(url.Query).AllKeys;

                foreach (var key in keys)
                {
                    var id = HttpUtility.ParseQueryString(url.Query).Get(key);

                    switch (key)
                    {
                        case "v":
                            result = await youtubeService
                                                .GetVideo(id)
                                                .ContinueWith((task) =>
                                                {
                                                    if (task.Exception != null)
                                                        throw task.Exception;

                                                    var videos = task.Result;
                                                    var tempResult = new DataParsingServiceResult(videos);

                                                    MediaItemsCache.Items.AddRange(videos);

                                                    foreach (var video in videos)
                                                        Messenger.Default.Send(video);

                                                    return tempResult;
                                                });

                            if (result != null)
                                return result;

                            continue;

                        case "list":
                            result = await youtubeService
                                                .GetPlaylist(id)
                                                .ContinueWith((task) =>
                                                {
                                                    if (task.Exception != null)
                                                        throw task.Exception;

                                                    var playlists = task.Result;
                                                    var tempResult = new DataParsingServiceResult(playlists);

                                                    PlaylistsCache.AddRange(playlists);

                                                    foreach (var playlist in playlists)
                                                        Messenger.Default.Send(playlist);

                                                    return tempResult;
                                                });

                            if (result != null)
                                return result;

                            continue;
                    }
                }

                match = match.NextMatch();
            }

            return new DataParsingServiceResult();
        }
    }
}
