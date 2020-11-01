using Maple.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Maple
{
    public static class ApplicationDbContextExtensions
    {
        public static void Insert(this ApplicationDbContext dbContext, AudioDeviceTypeModel viewModel)
        {
            var now = DateTime.Now;

            viewModel.CreatedOn = now;
            viewModel.CreatedBy = Environment.UserName;
            viewModel.UpdatedOn = now;
            viewModel.UpdatedBy = Environment.UserName;

            dbContext.AudioDeviceTypes.Add(viewModel);
        }

        public static void Insert(this ApplicationDbContext dbContext, MediaPlayer viewModel)
        {
            dbContext.Mediaplayers.Add(new Domain.MediaPlayerModel()
            {
                Name = viewModel.Name,
                Sequence = viewModel.Sequence,
                IsPrimary = viewModel.IsPrimary,
                PlaylistId = viewModel.Playlist?.Id,
                AudioDeviceId = viewModel.Playback?.AudioDevice?.Id,

                UpdatedOn = DateTime.UtcNow,
                UpdatedBy = Environment.UserName,
            });
        }

        public static void Insert(this ApplicationDbContext dbContext, Playlist viewModel)
        {
            dbContext.Playlists.Add(new Domain.PlaylistModel()
            {
                Name = viewModel.Name,
                Sequence = viewModel.Sequence,
                PrivacyStatus = viewModel.PrivacyStatus,
                RepeatMode = viewModel.RepeatMode,
                Thumbnail = viewModel.Thumbnail,

                UpdatedOn = DateTime.UtcNow,
                UpdatedBy = Environment.UserName,

                // TODO items
            });
        }

        public static void Delete(this ApplicationDbContext dbContext, MediaPlayer viewModel)
        {
            dbContext.Mediaplayers.Remove(new Domain.MediaPlayerModel()
            {
                Id = viewModel.Id
            });
        }

        public static void Delete(this ApplicationDbContext dbContext, Playlist viewModel)
        {
            dbContext.Playlists.Remove(new Domain.PlaylistModel()
            {
                Id = viewModel.Id
            });
        }

        public static void Delete(this ApplicationDbContext dbContext, MediaItem viewModel)
        {
            dbContext.MediaItems.Remove(new Domain.MediaItemModel()
            {
                Id = viewModel.Id
            });
        }

        public static async Task Update(this ApplicationDbContext dbContext, MediaPlayers mediaPlayers, CancellationToken token)
        {
            var query = dbContext.Mediaplayers.AsTracking().AsQueryable();
            foreach (var player in mediaPlayers.Items)
            {
                query = query.Where(p => player.Id == p.Id);
            }

            var players = await query.ToArrayAsync(token);
            var lookup = players.ToDictionary(p => p.Id);
            var now = DateTime.Now;

            foreach (var player in mediaPlayers.Items)
            {
                var model = lookup[player.Id];
                model.IsPrimary = player.IsPrimary;
                model.Name = player.Name;
                model.Sequence = player.Sequence;
                model.PlaylistId = player.Playlist?.Id;
                model.AudioDeviceId = player.Playback?.AudioDevice?.Id;

                model.UpdatedOn = now;
                model.UpdatedBy = Environment.UserName;
            }
        }

        public static async Task Save(this ApplicationDbContext dbContext, Playlists playlists, CancellationToken token)
        {
            var query = dbContext.Playlists.AsTracking().AsQueryable();
            foreach (var player in playlists.Items)
            {
                query = query.Where(p => player.Id == p.Id);
            }

            var players = await query.ToArrayAsync(token);
            var lookup = players.ToDictionary(p => p.Id);
            var now = DateTime.Now;

            foreach (var player in playlists.Items)
            {
                var model = lookup[player.Id];
                model.Name = player.Name;
                model.Sequence = player.Sequence;
                model.Thumbnail = player.Thumbnail;
                model.IsShuffeling = player.IsShuffeling;
                model.PrivacyStatus = player.PrivacyStatus;
                model.RepeatMode = player.RepeatMode;

                model.UpdatedOn = now;
                model.UpdatedBy = Environment.UserName;
            }
        }

        public static async Task Save(this ApplicationDbContext dbContext, AudioDevices audioDevices, CancellationToken token)
        {
            var query = dbContext.AudioDevices.AsTracking().AsQueryable();
            foreach (var player in audioDevices.Items)
            {
                query = query.Where(p => player.Id == p.Id);
            }

            var players = await query.ToArrayAsync(token);
            var lookup = players.ToDictionary(p => p.Id);
            var now = DateTime.Now;

            foreach (var player in audioDevices.Items)
            {
                var model = lookup[player.Id];
                model.Name = player.Name;
                model.Sequence = player.Sequence;
                // TODO model.AudioDeviceTypeId = player.DeviceType;

                model.UpdatedOn = now;
                model.UpdatedBy = Environment.UserName;
            }
        }
    }
}
