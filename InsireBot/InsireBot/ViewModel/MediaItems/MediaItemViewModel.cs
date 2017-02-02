using Maple.Core;
using Maple.Localization.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Maple
{
    public class MediaItemViewModel : TrackingBaseViewModel<Data.MediaItem>, IMediaItem, ISequence, IIdentifier, IValidatableTrackingObject, IValidatableObject
    {
        private readonly IBotLog _log;

        public int IdOriginalValue => GetOriginalValue<int>(nameof(Id));
        private int _id;
        public int Id
        {
            get { return _id; }
            private set { SetValue(ref _id, value); }
        }

        public int SequenceOriginalValue => GetOriginalValue<int>(nameof(Sequence));
        private int _sequence;
        public int Sequence
        {
            get { return _sequence; }
            set { SetValue(ref _sequence, value); }
        }

        public string TitleOriginalValue => GetOriginalValue<string>(nameof(Title));
        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetValue(ref _title, value); }
        }

        public TimeSpan DurationOriginalValue => GetOriginalValue<TimeSpan>(nameof(Duration));
        private TimeSpan _duration;
        public TimeSpan Duration
        {
            get { return _duration; }
            private set { SetValue(ref _duration, value); }
        }

        public PrivacyStatus PrivacyStatusOriginalValue => GetOriginalValue<PrivacyStatus>(nameof(PrivacyStatus));
        private PrivacyStatus _privacyStatus;
        public PrivacyStatus PrivacyStatus
        {
            get { return _privacyStatus; }
            private set { SetValue(ref _privacyStatus, value); }
        }

        public string LocationOriginalValue => GetOriginalValue<string>(nameof(Location));
        private string _location;
        public string Location
        {
            get { return _location; }
            private set { SetValue(ref _location, value); }
        }

        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set { SetValue(ref _isSelected, value); }
        }

        public MediaItemViewModel(IBotLog log, Data.MediaItem model) : base(model)
        {
            _log = log;
        }

        protected override void InitializeComplexProperties(Data.MediaItem model)
        {
            Id = model.Id;
            Location = model.Location;
            Title = model.Title;
            Sequence = model.Sequence;
            Duration = TimeSpan.FromSeconds(model.Duration);
            PrivacyStatus = (PrivacyStatus)model.PrivacyStatus;
        }

        public override string ToString()
        {
            var result = Title == string.Empty ? Location : Title;
            return result;
        }

        public override IEnumerable<ValidationResult> Validate(ValidationContext context)
        {
            if (string.IsNullOrWhiteSpace(Title))
                yield return new ValidationResult($"{nameof(Title)} {Resources.IsRequired}", new[] { nameof(Title) });

            if (string.IsNullOrWhiteSpace(Location))
                yield return new ValidationResult($"{nameof(Location)} {Resources.IsRequired}", new[] { nameof(Location) });
        }
    }
}
