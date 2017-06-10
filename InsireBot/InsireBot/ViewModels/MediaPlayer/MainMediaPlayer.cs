using FluentValidation;
using Maple.Core;
using Maple.Localization.Properties;

namespace Maple
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="Maple.MediaPlayer" />
    public class MainMediaPlayer : MediaPlayer
    {
        private const string _nameKey = nameof(Resources.MainMediaplayer);

        /// <summary>
        /// Initializes a new instance of the <see cref="MainMediaPlayer"/> class.
        /// </summary>
        /// <param name="manager">The manager.</param>
        /// <param name="player">The player.</param>
        /// <param name="model">The model.</param>
        /// <param name="playlist">The playlist.</param>
        /// <param name="devices">The devices.</param>
        public MainMediaPlayer(ILocalizationService manager, IMediaPlayer player, IValidator<MediaPlayer> validator, AudioDevices devices, Playlist playlist, Data.MediaPlayer model)
            : base(manager, player, validator, devices, playlist, model)
        {
            IsPrimary = model.IsPrimary;

            _manager.PropertyChanged += (o, e) =>
              {
                  if (e.PropertyName == nameof(ILocalizationService.CurrentLanguage))
                      UpdateName();
              };

            UpdateName();
        }

        private void UpdateName()
        {
            Name = _manager.Translate(_nameKey);
        }
    }
}
