using FluentValidation;
using Maple.Domain;

namespace Maple
{
    public class MainMediaPlayer : MediaPlayer
    {
        public MainMediaPlayer(IMapleCommandBuilder commandBuilder, IMediaPlayer player, IValidator<MediaPlayer> validator, AudioDevices devices, Playlist playlist, MediaPlayerModel model)
            : base(commandBuilder, player, validator, devices, playlist, model)
        {
            IsPrimary = model.IsPrimary;
        }
    }
}
