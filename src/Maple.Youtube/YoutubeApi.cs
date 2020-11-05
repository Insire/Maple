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

namespace Maple.Youtube
{
    internal sealed class YoutubeApi
    {
        private const string _videoBaseUrl = @"https://www.youtube.com/watch?v=";
        private const string _playListBaseUrl = @"https://www.youtube.com/playlist?list=";

        private volatile YouTubeService _service;

        private async Task<YouTubeService> GetService()
        {
            if (_service != null)
                return _service;

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

            return _service;
        }

        private async Task<UserCredential> GetCredential()
        {
            using (var stream = new FileStream(@"Resources\client_secret.json", FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                var secretCollection = GoogleClientSecrets.Load(stream);

                if (secretCollection is null || secretCollection.Secrets is null)
                {
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

        public async Task<ICollection<YoutubePlaylist>> GetPlaylists(string playlistId)
        {
            var result = new List<YoutubePlaylist>();
            var youtubeService = await GetService().ConfigureAwait(false);

            if (youtubeService is null)
            {
                return Enumerable.Empty<YoutubePlaylist>().ToList();
            }

            var request = youtubeService.Playlists.List("snippet,contentDetails");
            request.Id = playlistId;

            var response = await request.ExecuteAsync().ConfigureAwait(false);

            foreach (var item in response.Items)
            {
                var nextPageToken = "";
                while (nextPageToken != null)
                {
                    var playlist = new YoutubePlaylist
                    {
                        Title = item.Snippet.Title,
                        Location = $"{_playListBaseUrl}{item.Id}",
                        PrivacyStatus = string.IsNullOrEmpty(item.Status?.PrivacyStatus) ? 0 : 1,
                    };

                    result.Add(playlist);

                    nextPageToken = response.NextPageToken;
                }
            }

            return result;
        }

        public async Task CreatePlaylist(YoutubePlaylist playlist, bool publicPlaylist = true)
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

            foreach (var item in playlist.Items)
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

        public async Task DeletePlaylist(YoutubePlaylist playlist)
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

        private static string GetVideoId(YoutubeVideo item)
        {
            var url = new Uri(item.Location);
            var result = HttpUtility.ParseQueryString(url.Query)
                                    .Get("v");

            return result;
        }

        private static string GetPlaylistId(YoutubePlaylist list)
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

        public async Task<ICollection<YoutubeVideo>> GetVideo(string videoId)
        {
            var result = new List<YoutubeVideo>();
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
                    var video = new YoutubeVideo
                    {
                        Title = item.Snippet.Title,
                        Location = $"{_videoBaseUrl}{videoId}",
                        Duration = XmlConvert.ToTimeSpan(item.ContentDetails.Duration).Ticks,
                        PrivacyStatus = (item.ContentDetails.CountryRestriction?.Allowed ?? true) ? 0 : 1,
                    };

                    result.Add(video);

                    nextPageToken = response.NextPageToken;
                }
            }
            return result;
        }
    }
}
