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
        public MainMediaPlayer(ITranslationService manager, IMediaPlayer player, Data.MediaPlayer model, Playlist playlist, AudioDevices devices)
            : base(manager, player, model, playlist, devices)
        {
            IsPrimary = model.IsPrimary;

            _manager.PropertyChanged += (o, e) =>
              {
                  if (e.PropertyName == nameof(ITranslationService.CurrentLanguage))
                      UpdateName();
              };

            UpdateName();
        }

        private void IntializeValidation()
        {
            AddRule(IsPrimary, new NotFalseRule(nameof(IsPrimary)));
        }

        private void UpdateName()
        {
            Name = _manager.Translate(_nameKey);
        }
    }
}
