using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Xml;

using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;

using Maple.Domain;
using Maple.Localization.Properties;

namespace Maple.Youtube
{
    public class YoutubeApi : IYoutubeApi
    {
        private const string _videoBaseUrl = @"https://www.youtube.com/watch?v=";
        private const string _playListBaseUrl = @"https://www.youtube.com/playlist?list=";

        private volatile YouTubeService _service;
        private readonly ILoggingService _log;

        public YoutubeApi(ILoggingService log)
        {
            _log = log ?? throw new ArgumentNullException(nameof(log), $"{nameof(log)} {Resources.IsRequired}");
        }

        private async Task<YouTubeService> GetService()
        {
            if (_service != null)
                return _service;

            _log.Info(Resources.YoutubeLoad);

            var credentials = await GetCredential().ConfigureAwait(false);
            if (credentials is null)
            {
                return null;
            }

            _service = new YouTubeService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credentials,
                ApplicationName = GetType().ToString()
            });

            _log.Info(Resources.YoutubeLoaded);

            return _service;
        }

        private async Task<UserCredential> GetCredential()
        {
            using (var stream = new FileStream(@"Resources\client_secret.json", FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                var secretCollection = GoogleClientSecrets.Load(stream);

                if (secretCollection is null || secretCollection.Secrets is null)
                {
                    _log.Error("Youtube credentials not found!.");
                    return null;
                }

                var clientSecrets = secretCollection.Secrets;
                var store = new FileDataStore(GetType().ToString());
                var scopes = new[]
{
                    YouTubeService.Scope.YoutubeReadonly,
                    YouTubeService.Scope.Youtube
                };

                return await GoogleWebAuthorizationBroker.AuthorizeAsync(clientSecrets, scopes, "user", CancellationToken.None, store)
                                                         .ConfigureAwait(false);
            }
        }

        public async Task<ICollection<PlaylistModel>> GetPlaylists(string playlistId)
        {
            var result = new List<PlaylistModel>();
            var youtubeService = await GetService().ConfigureAwait(false);

            if (youtubeService is null)
            {
                return Enumerable.Empty<PlaylistModel>().ToList();
            }

            var request = youtubeService.Playlists.List("snippet,contentDetails");
            request.Id = playlistId;

            var response = await request.ExecuteAsync().ConfigureAwait(false);

            foreach (var item in response.Items)
            {
                var nextPageToken = "";
                while (nextPageToken != null)
                {
                    var playlist = new Domain.PlaylistModel
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

        public async Task CreatePlaylist(PlaylistModel playlist, bool publicPlaylist = true)
        {
            var youtubeService = await GetService().ConfigureAwait(false);

            if (youtubeService is null)
            {
                return;
            }

            var newPlaylist = new Playlist()
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
            newPlaylist = await youtubeService.Playlists.Insert(newPlaylist, "snippet,status")
                                                        .ExecuteAsync()
                                                        .ConfigureAwait(false);

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
                newVideo = await youtubeService.PlaylistItems.Insert(newVideo, "snippet")
                                                             .ExecuteAsync()
                                                             .ConfigureAwait(false);
            }
        }

        public async Task DeletePlaylist(PlaylistModel playlist)
        {
            var youtubeService = await GetService().ConfigureAwait(false);
            if (youtubeService is null)
            {
                return;
            }

            var id = GetPlaylistId(playlist);

            await youtubeService.Playlists.Delete(id)
                                          .ExecuteAsync()
                                          .ConfigureAwait(false);
        }

        public static string GetVideoId(MediaItemModel item)
        {
            var url = new Uri(item.Location);
            var result = HttpUtility.ParseQueryString(url.Query)
                                    .Get("v");

            return result;
        }

        public static string GetPlaylistId(PlaylistModel list)
        {
            var url = new Uri(list.Location);
            var result = HttpUtility.ParseQueryString(url.Query)
                                    .Get("list");

            return result;
        }

        public async Task<ICollection<PlaylistItem>> GetPlaylistItems(string playlistId)
        {
            var youtubeService = await GetService().ConfigureAwait(false);
            if (youtubeService is null)
            {
                return Enumerable.Empty<PlaylistItem>().ToList();
            }

            var result = new List<PlaylistItem>();

            var request = youtubeService.PlaylistItems.List("snippet,contentDetails");
            request.PlaylistId = playlistId;

            var response = await request.ExecuteAsync()
                                        .ConfigureAwait(false);

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

        public async Task DeletePlaylistItems(ICollection<PlaylistItem> playlistItems)
        {
            var youtubeService = await GetService().ConfigureAwait(false);
            if (youtubeService is null)
            {
                return;
            }

            foreach (var item in playlistItems)
                await youtubeService.PlaylistItems.Delete(item.Id)
                                                  .ExecuteAsync()
                                                  .ConfigureAwait(false);
        }

        //TODO writing a async sync method for what i get from youtube vs that i generate myself as playlist

        public async Task<ICollection<MediaItemModel>> GetVideo(string videoId)
        {
            var result = new List<MediaItemModel>();
            var youtubeService = await GetService().ConfigureAwait(false);

            var request = youtubeService.Videos.List("snippet,contentDetails");
            request.Id = videoId;

            var response = await request.ExecuteAsync()
                                        .ConfigureAwait(false);

            foreach (var item in response.Items)
            {
                var nextPageToken = "";
                while (nextPageToken != null)
                {
                    var video = new Domain.MediaItemModel
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
