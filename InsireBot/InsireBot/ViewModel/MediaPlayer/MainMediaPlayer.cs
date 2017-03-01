using Maple.Localization.Properties;
using System;

namespace Maple
{
    public class MainMediaPlayer : MediaPlayer
    {
        private readonly ITranslationManager _manager;
        private readonly string _nameKey;

        public MainMediaPlayer(ITranslationManager manager, IMediaPlayer player, Data.MediaPlayer model, Playlist playlist, string nameKey)
            : base(manager, player, model)
        {
            if (string.IsNullOrWhiteSpace(nameKey))
                throw new ArgumentNullException(nameof(nameKey), $"{nameof(nameKey)} {Resources.IsRequired}");

            _manager = manager;
            _nameKey = nameKey;

            Name = model.Name;
            IsPrimary = model.IsPrimary;
            Playlist = playlist ?? throw new ArgumentNullException(nameof(playlist), $"{nameof(playlist)} {Resources.IsRequired}");

            _manager.PropertyChanged += (o, e) =>
              {
                  if (e.PropertyName == nameof(ITranslationManager.CurrentLanguage))
                      UpdateName();
              };

            UpdateName();
        }

        //public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        //{
        //    if (string.IsNullOrWhiteSpace(Name))
        //        yield return new ValidationResult($"{nameof(Name)} {Resources.IsRequired}", new[] { nameof(Name) });


        //    if (!IsPrimary)
        //        yield return new ValidationResult($"{nameof(IsPrimary)} {Resources.IsRequired}", new[] { nameof(IsPrimary) });
        //}

        private void UpdateName()
        {
            Name = _manager.Translate(_nameKey);
        }
    }
}
