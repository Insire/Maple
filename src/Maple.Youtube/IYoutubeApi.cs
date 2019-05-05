using System.Collections.Generic;
using System.Threading.Tasks;

using Google.Apis.YouTube.v3.Data;

using Maple.Domain;

namespace Maple.Youtube
{
    public interface IYoutubeApi
    {
        Task CreatePlaylist(PlaylistModel playlist, bool publicPlaylist = true);

        Task DeletePlaylist(PlaylistModel playlist);

        Task DeletePlaylistItems(ICollection<PlaylistItem> playlistItems);

        Task<ICollection<PlaylistItem>> GetPlaylistItems(string playlistId);

        Task<ICollection<PlaylistModel>> GetPlaylists(string playlistId);

        Task<ICollection<MediaItemModel>> GetVideo(string videoId);
    }
}
