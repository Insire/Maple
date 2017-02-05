using Maple.Data;
using Maple.Localization.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Maple
{
    public class MainMediaPlayer : MediaPlayer
    {
        private readonly ITranslationManager _manager;
        private readonly string _nameKey;

        public MainMediaPlayer(ITranslationManager manager, IMediaPlayerRepository mediaPlayerRepository, IMediaPlayer player, Data.MediaPlayer model,Playlist playlist, string nameKey)
            : base(manager, mediaPlayerRepository, player, model)
        {
            if (string.IsNullOrWhiteSpace(nameKey))
                throw new ArgumentNullException(nameof(nameKey), $"{nameof(nameKey)} {Resources.IsRequired}");

            _manager = manager;
            _nameKey = nameKey;

            Name = model.Name;
            IsPrimary = model.IsPrimary;
            Playlist = playlist;

            _manager.PropertyChanged += (o, e) =>
              {
                  if (e.PropertyName == nameof(ITranslationManager.CurrentLanguage))
                      UpdateName();
              };

            UpdateName();

            if (!Model.IsNew)
                AcceptChanges();

            Validate();
        }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(Name))
                yield return new ValidationResult($"{nameof(Name)} {Resources.IsRequired}", new[] { nameof(Name) });


            if (!IsPrimary)
                yield return new ValidationResult($"{nameof(IsPrimary)} {Resources.IsRequired}", new[] { nameof(IsPrimary) });
        }

        private void UpdateName()
        {
            Name = _manager.Translate(_nameKey);
        }
    }
}
