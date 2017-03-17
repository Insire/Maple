using Maple.Core;
using Maple.Localization.Properties;

namespace Maple
{
    public class MainMediaPlayer : MediaPlayer
    {
        private const string _nameKey = nameof(Resources.MainMediaplayer);

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
