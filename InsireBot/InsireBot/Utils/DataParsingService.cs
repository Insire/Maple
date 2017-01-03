using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace InsireBot
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

        public DataParsingService()
        {
            PlaylistsCache = new PlaylistsStore();
            MediaItemsCache = new MediaItemsStore();
        }

        public DataParsingServiceResult Parse(FileInfo file)
        {
            //assume its a MediaFile aka *.mp3 or something similar
            var mediaItem = new MediaItem(Path.GetFileNameWithoutExtension(file.FullName), new Uri(file.FullName));
            var result = new DataParsingServiceResult(mediaItem);
            Messenger.Default.Send(mediaItem);

            return result;
        }

        public async Task<DataParsingServiceResult> Parse(string data, DataParsingServiceResultType type)
        {
            var url = new Uri(data.Trim());
            try
            {
                switch (url.DnsSafeHost)
                {
                    case "youtu.be":
                        url = new Uri(url.AbsoluteUri.Replace(@"youtu.be/", @"youtube.com/watch?v="));
                        return await Parse(url, type);

                    case "www.youtube.com":
                        return await Parse(url, type);

                    default:
                        return new DataParsingServiceResult();
                }
            }
            catch (Exception ex)
            {
                App.Log.Error(this, ex);
                return new DataParsingServiceResult();
            }
        }

        private async Task<DataParsingServiceResult> Parse(Uri url, DataParsingServiceResultType type = DataParsingServiceResultType.None)
        {
            var result = new DataParsingServiceResult(type);
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
                            await youtubeService.GetVideo(id)
                                                .ContinueWith((task) =>
                                                {
                                                    if (task.Exception != null)
                                                        App.Log.Error(task.Exception);

                                                    var videos = task.Result;

                                                    MediaItemsCache.Items.AddRange(videos);

                                                    foreach (var video in videos)
                                                    {
                                                        result.MediaItems.Add(video);
                                                        Messenger.Default.Send(video);
                                                    }
                                                });
                            break;

                        case "list":
                            await youtubeService.GetPlaylists(id)
                                                .ContinueWith((task) =>
                                                {
                                                    if (task.Exception != null)
                                                        App.Log.Error(task.Exception);

                                                    var playlists = task.Result;

                                                    PlaylistsCache.Items.AddRange(playlists);

                                                    foreach (var playlist in playlists)
                                                    {
                                                        result.Playlists.Add(playlist);
                                                        Messenger.Default.Send(playlist);
                                                    }
                                                });
                            break;
                    }
                }

                match = match.NextMatch();
            }

            return result;
        }
    }
}
