using Maple.Data;
using Maple.Localization.Properties;
using System;

namespace Maple
{
    public class MainMediaPlayer : MediaPlayer
    {
        private readonly string _nameKey;

        public MainMediaPlayer(PlaylistContext context, ITranslationManager manager, IMediaPlayer player, Data.MediaPlayer model, Playlist playlist, AudioDevices devices, string nameKey)
            : base(context, manager, player, model, playlist, devices)
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

        private void InitializeValidation()
        {

        }
    }
}
