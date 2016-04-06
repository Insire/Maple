using System.Threading.Tasks;
using System.IO;
using System.Threading;
using System;

using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Google.Apis.YouTube.v3;
using System.Collections.Generic;
using Google.Apis.YouTube.v3.Data;
using System.Xml;
using InsireBot.MediaPlayer;

namespace InsireBot
{
    public class Youtube
    {
        private string _videoBaseUrl = @"https://www.youtube.com/watch?v=";

        internal async Task<YouTubeService> GetService()
        {
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

            return new YouTubeService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = GetType().ToString()
            });
        }

        //public async Task<IEnumerable<MediaItem>> GetUploads()
        //{
        //    var result = new List<MediaItem>();
        //    var youtubeService = await GetService();

        //    var channelsListRequest = youtubeService.Channels.List("contentDetails");
        //    channelsListRequest.Mine = true;

        //    // Retrieve the contentDetails part of the channel resource for the authenticated user's channel.
        //    var channelsListResponse = await channelsListRequest.ExecuteAsync();

        //    foreach (var channel in channelsListResponse.Items)
        //    {
        //        // From the API response, extract the playlist ID that identifies the list
        //        // of videos uploaded to the authenticated user's channel.
        //        var uploadsListId = channel.ContentDetails.RelatedPlaylists.Uploads;

        //        //Console.WriteLine("Videos in list {0}", uploadsListId);

        //        var nextPageToken = "";
        //        while (nextPageToken != null)
        //        {
        //            var playlistItemsListRequest = youtubeService.PlaylistItems.List("snippet");
        //            playlistItemsListRequest.PlaylistId = uploadsListId;
        //            playlistItemsListRequest.MaxResults = 50;
        //            playlistItemsListRequest.PageToken = nextPageToken;

        //            // Retrieve the list of videos uploaded to the authenticated user's channel.
        //            var playlistItemsListResponse = await playlistItemsListRequest.ExecuteAsync();

        //            foreach (var playlistItem in playlistItemsListResponse.Items)
        //            {
        //                // Print information about each video.
        //                var location = new Uri($"{_videoBaseUrl}{playlistItem.Snippet.ResourceId.VideoId}");
        //                result.Add(new MediaItem(playlistItem.Snippet.Title, location));
        //            }

        //            nextPageToken = playlistItemsListResponse.NextPageToken;
        //        }
        //    }
        //    return result;
        //}

        //public async Task CreatePlaylist(string title, IEnumerable<MediaItem> videos)
        //{
        //    var youtubeService = await GetService();

        //    // Create a new, private playlist in the authorized user's channel.
        //    var newPlaylist = new Google.Apis.YouTube.v3.Data.Playlist();
        //    newPlaylist.Snippet = new PlaylistSnippet();
        //    newPlaylist.Snippet.Title = "Test Playlist";
        //    newPlaylist.Snippet.Description = "A playlist created with the YouTube API v3";
        //    newPlaylist.Status = new PlaylistStatus();
        //    newPlaylist.Status.PrivacyStatus = "public";
        //    newPlaylist = await youtubeService.Playlists.Insert(newPlaylist, "snippet,status").ExecuteAsync();

        //    // Add a video to the newly created playlist.
        //    var newPlaylistItem = new PlaylistItem();
        //    newPlaylistItem.Snippet = new PlaylistItemSnippet();
        //    newPlaylistItem.Snippet.PlaylistId = newPlaylist.Id;
        //    newPlaylistItem.Snippet.ResourceId = new ResourceId();
        //    newPlaylistItem.Snippet.ResourceId.Kind = "youtube#video";
        //    newPlaylistItem.Snippet.ResourceId.VideoId = "GNRMeaz6QRI";
        //    newPlaylistItem = await youtubeService.PlaylistItems.Insert(newPlaylistItem, "snippet").ExecuteAsync();

        //    //Console.WriteLine("Playlist item id {0} was added to playlist id {1}.", newPlaylistItem.Id, newPlaylist.Id);
        //}

        public async Task<IEnumerable<MediaItem>> GetPlaylistItems(string playlistId)
        {
            var result = new List<MediaItem>();
            var youtubeService = await GetService();

            var request = youtubeService.PlaylistItems.List("snippet,contentDetails");
            request.PlaylistId = playlistId;

            var response = await request.ExecuteAsync();

            foreach (var item in response.Items)
            {
                var nextPageToken = "";
                while (nextPageToken != null)
                {
                    var videos = await GetVideo(item.ContentDetails.VideoId);

                    foreach (var video in videos)
                        result.Add(video);

                    nextPageToken = response.NextPageToken;
                }
            }
            return result;
        }

        public async Task<IEnumerable<MediaPlayer.Playlist>> GetPlaylist(string playlistId)
        {
            var result = new List<MediaPlayer.Playlist>();
            var youtubeService = await GetService();

            var request = youtubeService.Playlists.List("snippet,contentDetails");
            request.Id = playlistId;

            var response = await request.ExecuteAsync();

            foreach (var item in response.Items)
            {
                var nextPageToken = "";
                while (!string.IsNullOrEmpty(nextPageToken))
                {
                    var playlist = new MediaPlayer.Playlist(item.Snippet.Title, item.Id);
                    var videos = await GetPlaylistItems(item.Id);

                    playlist.AddRange(videos);
                    result.Add(playlist);

                    nextPageToken = response.NextPageToken;
                }
            }

            return result;
        }

        public async Task<IEnumerable<MediaItem>> GetVideo(string videoId)
        {
            var result = new List<MediaItem>();
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
