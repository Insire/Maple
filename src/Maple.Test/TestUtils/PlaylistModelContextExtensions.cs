using System;
using System.Collections.Generic;

using Maple.Domain;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Maple.Test
{
    public static class PlaylistModelContextExtensions
    {
        public static PlaylistModel CreateModelPlaylist(this TestContext context)
        {
            var playlist = new PlaylistModel()
            {
                CreatedBy = context.FullyQualifiedTestClassName,
                CreatedOn = DateTime.UtcNow,
                Description = $"Description for {context.FullyQualifiedTestClassName} Playlist",
                Id = 1,
                IsDeleted = false,
                IsShuffeling = false,
                Location = "Memory",
                MediaItems = new List<MediaItemModel>(),
                PrivacyStatus = (int)PrivacyStatus.None,
                RepeatMode = (int)RepeatMode.None,
                Sequence = 1,
                Title = $"Title for {context.FullyQualifiedTestClassName} Playlist",
                UpdatedBy = context.FullyQualifiedTestClassName,
                UpdatedOn = DateTime.UtcNow,
            };

            return PopulatePlaylist(context, playlist);
        }

        public static PlaylistModel PopulatePlaylist(this TestContext context, PlaylistModel playlist)
        {
            for (var i = 0; i < 4; i++)
            {
                playlist.MediaItems.Add(new MediaItemModel()
                {
                    CreatedBy = context.FullyQualifiedTestClassName,
                    CreatedOn = DateTime.UtcNow,
                    UpdatedOn = DateTime.UtcNow,
                    UpdatedBy = context.FullyQualifiedTestClassName,
                    Description = $"Description for {context.FullyQualifiedTestClassName} MediaItem number {i}",
                    Duration = 0,
                    Id = 1,
                    IsDeleted = false,
                    Location = "Memory",
                    Playlist = playlist,
                    PlaylistId = playlist.Id,
                    PrivacyStatus = (int)PrivacyStatus.None,
                    Sequence = 0,
                    Title = $"Title for {context.FullyQualifiedTestClassName} MediaItem number {i}",
                });
            }

            return playlist;
        }
    }
}
