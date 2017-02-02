using Maple.Localization.Properties;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Maple
{
    public class MainMediaPlayer : MediaPlayer
    {
        private readonly ITranslationManager _manager;
        private readonly string _nameKey;

        public MainMediaPlayer(ITranslationManager manager, IMediaPlayer player, Data.MediaPlayer mediaPlayer, string nameKey) : base(player, mediaPlayer)
        {
            _manager = manager;
            _nameKey = nameKey;

            _manager.PropertyChanged += (o, e) =>
              {
                  if (e.PropertyName == nameof(ITranslationManager.CurrentLanguage))
                      UpdateName();
              };

            UpdateName();

            if (!Model.IsNew)
                AcceptChanges();
        }

        protected override void InitializeComplexProperties(Data.MediaPlayer model)
        {
            Name = model.Name;
            IsPrimary = model.IsPrimary;
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
