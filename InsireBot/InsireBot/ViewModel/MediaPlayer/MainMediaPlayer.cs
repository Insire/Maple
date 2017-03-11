using Maple.Core;
using Maple.Localization.Properties;
using System;

namespace Maple
{
    public class MainMediaPlayer : MediaPlayer
    {
        private readonly string _nameKey;

        public MainMediaPlayer(ITranslationManager manager, IMediaPlayer player, Data.MediaPlayer model, Playlist playlist, AudioDevices devices, string nameKey)
            : base(manager, player, model, playlist, devices)
        {
            if (string.IsNullOrWhiteSpace(nameKey))
                throw new ArgumentNullException(nameof(nameKey), $"{nameof(nameKey)} {Resources.IsRequired}");

            _nameKey = nameKey;

            IsPrimary = model.IsPrimary;

            _manager.PropertyChanged += (o, e) =>
              {
                  if (e.PropertyName == nameof(ITranslationManager.CurrentLanguage))
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
