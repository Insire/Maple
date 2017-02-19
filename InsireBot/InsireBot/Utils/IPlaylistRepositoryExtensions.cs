using Maple.Data;
using Maple.Localization.Properties;
using System.Collections.Generic;

namespace Maple
{
    public static class IPlaylistRepositoryExtensions
    {
        public static List<Data.Playlist> Seed(this IPlaylistsRepository repository)
        {
            var playlists = repository.GetAll();

            if (playlists.Count <= 0)
            {
                var list = repository.Create(new Data.Playlist
                {
                    Title = Resources.Title,
                });
                playlists.Add(list);
            }

            return playlists;
        }
    }
}
