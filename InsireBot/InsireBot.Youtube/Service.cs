using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using Maple.Core;
using Maple.Localization.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Xml;

namespace Maple.Youtube
{
    public class Service
    {
        private const string _videoBaseUrl = @"https://www.youtube.com/watch?v=";
        private const string _playListBaseUrl = @"https://www.youtube.com/playlist?list=";

        private YouTubeService _service;
        private readonly IMapleLog _log;
        public Service(IMapleLog log)
        {
            _log = log;
        }

        private async Task<YouTubeService> GetService()
        {
            if (_service == null)
            {
                _log.Info(Resources.YoutubeLoad);

                UserCredential credential;
                using (var stream = new FileStream(@"Resources\client_secret.json", FileMode.Open, FileAccess.Read, FileShare.Read))
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

                _log.Info(Resources.YoutubeLoaded);
                _service = result;
                return result;
            }
            else
            {
                return _service;
            }
        }

        public async Task<List<Core.Playlist>> GetPlaylists(string playlistId)
        {
            var result = new List<Core.Playlist>();
            var youtubeService = await GetService();

            var request = youtubeService.Playlists.List("snippet,contentDetails");
            request.Id = playlistId;

            var response = await request.ExecuteAsync();

            foreach (var item in response.Items)
            {
                var nextPageToken = "";
                while (nextPageToken != null)
                {
                    var playlist = new Core.Playlist
                    {
                        Title = item.Snippet.Title,
                        Location = $"{_playListBaseUrl}{item.Id}",
                        PrivacyStatus = string.IsNullOrEmpty(item.Status?.PrivacyStatus) ? (int)PrivacyStatus.None : (int)PrivacyStatus.Restricted,
                    };

                    result.Add(playlist);

                    nextPageToken = response.NextPageToken;
                }
            }

            return result;
        }

        public async Task CreatePlaylist(Core.Playlist playlist, bool publicPlaylist = true)
        {
            var youtubeService = await GetService();

            var newPlaylist = new Google.Apis.YouTube.v3.Data.Playlist()
            {
                Snippet = new PlaylistSnippet()
                {
                    Title = playlist.Title,
                    Description = playlist.Description
                },
                Status = new PlaylistStatus
                {
                    PrivacyStatus = publicPlaylist == true ? "public" : "private"
                }
            };
            newPlaylist = await youtubeService.Playlists.Insert(newPlaylist, "snippet,status").ExecuteAsync();

            foreach (var item in playlist.MediaItems)
            {
                // Add a video to the newly created playlist.
                var newVideo = new PlaylistItem()
                {
                    Snippet = new PlaylistItemSnippet()
                    {
                        PlaylistId = newPlaylist.Id,
                        ResourceId = new ResourceId()
                    }
                };
                newVideo.Snippet.ResourceId.Kind = "youtube#video";
                newVideo.Snippet.ResourceId.VideoId = GetVideoId(item);
                newVideo = await youtubeService.PlaylistItems.Insert(newVideo, "snippet").ExecuteAsync();
            }
        }

        public async Task DeletePlaylist(Core.Playlist playlist)
        {
            var youtubeService = await GetService();
            var id = GetPlaylistId(playlist);
            await youtubeService.Playlists.Delete(id).ExecuteAsync();
        }

        public static string GetVideoId(Core.MediaItem item)
        {
            var url = new Uri(item.Location);
            var result = HttpUtility.ParseQueryString(url.Query).Get("v");
            return result;
        }

        public static string GetVideoId(Data.MediaItem item)
        {
            var url = new Uri(item.Location);
            var result = HttpUtility.ParseQueryString(url.Query).Get("v");
            return result;
        }

        public static string GetPlaylistId(Core.Playlist list)
        {
            var url = new Uri(list.Location);
            var result = HttpUtility.ParseQueryString(url.Query).Get("list");
            return result;
        }

        public static string GetPlaylistId(Data.Playlist list)
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

        public async Task<IList<Core.MediaItem>> GetVideo(string videoId)
        {
            var result = new List<Core.MediaItem>();
            var youtubeService = await GetService();

            var request = youtubeService.Videos.List("snippet,contentDetails");
            request.Id = videoId;

            var response = await request.ExecuteAsync();

            foreach (var item in response.Items)
            {
                var nextPageToken = "";
                while (nextPageToken != null)
                {
                    var video = new Core.MediaItem
                    {
                        Title = item.Snippet.Title,
                        Location = $"{_videoBaseUrl}{videoId}",
                        Duration = XmlConvert.ToTimeSpan(item.ContentDetails.Duration).Ticks,
                        PrivacyStatus = (item.ContentDetails.CountryRestriction?.Allowed ?? true) ? (int)PrivacyStatus.None : (int)PrivacyStatus.Restricted,
                    };

                    result.Add(video);

                    nextPageToken = response.NextPageToken;
                }
            }
            return result;
        }
    }
}
