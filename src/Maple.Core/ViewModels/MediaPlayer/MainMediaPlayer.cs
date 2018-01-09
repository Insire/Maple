using FluentValidation;
using Maple.Domain;
using Maple.Localization.Properties;

namespace Maple.Core
{
    public class MainMediaPlayer : MediaPlayer
    {
        private const string _nameKey = nameof(Resources.MainMediaplayer);

        public MainMediaPlayer(ViewModelServiceContainer container, IMediaPlayer player, IValidator<MediaPlayer> validator, AudioDevices devices, Playlist playlist, MediaPlayerModel model)
            : base(container, player, validator, devices, playlist, model)
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
