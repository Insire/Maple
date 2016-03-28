using System.Threading.Tasks;
using System.IO;
using System.Threading;

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
            using (var stream = new FileStream("client_secret.json", FileMode.Open, FileAccess.Read))
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

        public async Task<IEnumerable<MediaItem>> GetUploads()
        {
            var result = new List<MediaItem>();
            var youtubeService = await GetService();

            var channelsListRequest = youtubeService.Channels.List("contentDetails");
            channelsListRequest.Mine = true;

            // Retrieve the contentDetails part of the channel resource for the authenticated user's channel.
            var channelsListResponse = await channelsListRequest.ExecuteAsync();

            foreach (var channel in channelsListResponse.Items)
            {
                // From the API response, extract the playlist ID that identifies the list
                // of videos uploaded to the authenticated user's channel.
                var uploadsListId = channel.ContentDetails.RelatedPlaylists.Uploads;

                //Console.WriteLine("Videos in list {0}", uploadsListId);

                var nextPageToken = "";
                while (nextPageToken != null)
                {
                    var playlistItemsListRequest = youtubeService.PlaylistItems.List("snippet");
                    playlistItemsListRequest.PlaylistId = uploadsListId;
                    playlistItemsListRequest.MaxResults = 50;
                    playlistItemsListRequest.PageToken = nextPageToken;

                    // Retrieve the list of videos uploaded to the authenticated user's channel.
                    var playlistItemsListResponse = await playlistItemsListRequest.ExecuteAsync();

                    foreach (var playlistItem in playlistItemsListResponse.Items)
                    {
                        // Print information about each video.
                        result.Add(new MediaItem(playlistItem.Snippet.Title, playlistItem.Snippet.ResourceId.VideoId));
                    }

                    nextPageToken = playlistItemsListResponse.NextPageToken;
                }
            }
            return result;
        }

        public async Task CreatePlaylist(string title, IEnumerable<MediaItem> videos)
        {
            var youtubeService = await GetService();

            // Create a new, private playlist in the authorized user's channel.
            var newPlaylist = new Playlist();
            newPlaylist.Snippet = new PlaylistSnippet();
            newPlaylist.Snippet.Title = "Test Playlist";
            newPlaylist.Snippet.Description = "A playlist created with the YouTube API v3";
            newPlaylist.Status = new PlaylistStatus();
            newPlaylist.Status.PrivacyStatus = "public";
            newPlaylist = await youtubeService.Playlists.Insert(newPlaylist, "snippet,status").ExecuteAsync();

            // Add a video to the newly created playlist.
            var newPlaylistItem = new PlaylistItem();
            newPlaylistItem.Snippet = new PlaylistItemSnippet();
            newPlaylistItem.Snippet.PlaylistId = newPlaylist.Id;
            newPlaylistItem.Snippet.ResourceId = new ResourceId();
            newPlaylistItem.Snippet.ResourceId.Kind = "youtube#video";
            newPlaylistItem.Snippet.ResourceId.VideoId = "GNRMeaz6QRI";
            newPlaylistItem = await youtubeService.PlaylistItems.Insert(newPlaylistItem, "snippet").ExecuteAsync();

            //Console.WriteLine("Playlist item id {0} was added to playlist id {1}.", newPlaylistItem.Id, newPlaylist.Id);
        }


        public async Task<IEnumerable<MediaItem>> GetVideo(string id)
        {
            var result = new List<MediaItem>();
            var youtubeService = await GetService();

            var videosListRequest = youtubeService.Videos.List("snippet,contentDetails");
            videosListRequest.Id = id;


            var channelsListResponse = await videosListRequest.ExecuteAsync();

            foreach (var video in channelsListResponse.Items)
            {
                var nextPageToken = "";
                while (nextPageToken != null)
                {
                    result.Add(new MediaItem(video.Snippet.Title,
                                            $"{_videoBaseUrl}{id}",
                                            XmlConvert.ToTimeSpan(video.ContentDetails.Duration),
                                            video.ContentDetails.CountryRestriction?.Allowed));

                    nextPageToken = channelsListResponse.NextPageToken;
                }
            }
            return result;
        }
    }
}
