using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Xml;

namespace InsireBot
{
    public class Youtube
    {
        private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private string _videoBaseUrl = @"https://www.youtube.com/watch?v=";
        private YouTubeService _service;

        private async Task<YouTubeService> GetService()
        {
            if (_service == null)
            {
                _log.Info("Connecting to Youtube");

                UserCredential credential;
                using (var stream = new FileStream(@"Resources\client_secret.json", FileMode.Open, FileAccess.Read))
                {
                    credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                        GoogleClientSecrets.Load(stream).Secrets,
                        new[]
                        {
                            YouTubeService.Scope.YoutubeReadonly,
                            YouTubeService.Scope.Youtube
                        },
                        "user",
                        CancellationToken.None,
                        new FileDataStore(GetType().ToString())
                    );
                }

                var result = new YouTubeService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = GetType().ToString()
                });

                _log.Info("Connected to Youtube");
                _service = result;
                return result;
            }
            else
            {
                return _service;
            }
        }

        public async Task<IList<Playlist<IMediaItem>>> GetPlaylists(string playlistId)
        {
            var result = new List<Playlist<IMediaItem>>();
            var youtubeService = await GetService();

            var request = youtubeService.Playlists.List("snippet,contentDetails");
            request.Id = playlistId;

            var response = await request.ExecuteAsync();

            foreach (var item in response.Items)
            {
                var nextPageToken = "";
                while (nextPageToken != null)
                {
                    var playlist = new Playlist<IMediaItem>(item.Snippet.Title, $"https://www.youtube.com/playlist?list={item.Id}", item.ContentDetails.ItemCount ?? 0, item.Status?.PrivacyStatus ?? "none");

                    result.Add(playlist);

                    nextPageToken = response.NextPageToken;
                }
            }

            return result;
        }

        public async Task CreatePlaylist(Playlist<IMediaItem> playlist, bool publicPlaylist = true)
        {
            var youtubeService = await GetService();

            var newPlaylist = new Playlist();
            newPlaylist.Snippet = new PlaylistSnippet();
            newPlaylist.Snippet.Title = playlist.Title;
            newPlaylist.Snippet.Description = playlist.Description;
            newPlaylist.Status = new PlaylistStatus
            {
                PrivacyStatus = publicPlaylist == true ? "public" : "private"
            };

            newPlaylist = await youtubeService.Playlists.Insert(newPlaylist, "snippet,status").ExecuteAsync();

            foreach (var item in playlist)
            {
                // Add a video to the newly created playlist.
                var newVideo = new PlaylistItem();
                newVideo.Snippet = new PlaylistItemSnippet();
                newVideo.Snippet.PlaylistId = newPlaylist.Id;
                newVideo.Snippet.ResourceId = new ResourceId();
                newVideo.Snippet.ResourceId.Kind = "youtube#video";
                newVideo.Snippet.ResourceId.VideoId = GetVideoId(item);
                newVideo = await youtubeService.PlaylistItems.Insert(newVideo, "snippet").ExecuteAsync();
            }
        }

        public async Task DeletePlaylist(Playlist<IMediaItem> playlist)
        {
            var youtubeService = await GetService();
            var id = GetPlaylistId(playlist);
            await youtubeService.Playlists.Delete(id).ExecuteAsync();
        }

        public static string GetVideoId(IMediaItem item)
        {
            var url = new Uri(item.Location);
            var result = HttpUtility.ParseQueryString(url.Query).Get("v");
            return result;
        }

        public static string GetPlaylistId(Playlist<IMediaItem> list)
        {
            var url = new Uri(list.Location);
            var result = HttpUtility.ParseQueryString(url.Query).Get("list");
            return result;
        }

        public async Task<IList<PlaylistItem>> GetPlaylistItems(string playlistId)
        {
            var result = new List<PlaylistItem>();
            var youtubeService = await GetService();

            var request = youtubeService.PlaylistItems.List("snippet,contentDetails");
            request.PlaylistId = playlistId;
            var response = await request.ExecuteAsync();

            foreach (var item in response.Items)
            {
                var nextPageToken = "";
                while (nextPageToken != null)
                {

                    result.Add(item);

                    nextPageToken = response.NextPageToken;
                }
            }
            return result;
        }

        public async Task DeletePlaylistItems(IList<PlaylistItem> playlistItems)
        {
            var youtubeService = await GetService();

            foreach (var item in playlistItems)
                await youtubeService.PlaylistItems.Delete(item.Id).ExecuteAsync();
        }

        //TODO writing a async sync method for what i get from youtube vs that i generate myself as playlist

        public async Task<IList<IMediaItem>> GetVideo(string videoId)
        {
            var result = new List<IMediaItem>();
            var youtubeService = await GetService();

            var request = youtubeService.Videos.List("snippet,contentDetails");
            request.Id = videoId;

            var response = await request.ExecuteAsync();

            foreach (var item in response.Items)
            {
                var nextPageToken = "";
                while (nextPageToken != null)
                {
                    var video = new MediaItem(item.Snippet.Title,
                                            new Uri($"{_videoBaseUrl}{videoId}"),
                                            XmlConvert.ToTimeSpan(item.ContentDetails.Duration),
                                            item.ContentDetails.CountryRestriction?.Allowed);
                    result.Add(video);

                    nextPageToken = response.NextPageToken;
                }
            }
            return result;
        }
    }
}
