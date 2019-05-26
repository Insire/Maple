using FluentValidation;
using Maple.Domain;
using Maple.Localization.Properties;

namespace Maple
{
    public class MainMediaPlayer : MediaPlayer
    {
        private const string _nameKey = nameof(Resources.MainMediaplayer);

        public MainMediaPlayer(IMapleCommandBuilder commandBuilder, IMediaPlayer player, IValidator<MediaPlayer> validator, AudioDevices devices, Playlist playlist, MediaPlayerModel model)
            : base(commandBuilder, player, validator, devices, playlist, model)
        {
            IsPrimary = model.IsPrimary;
        }
    }
}
