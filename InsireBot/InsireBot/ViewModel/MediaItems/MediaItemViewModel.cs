﻿using InsireBot.Core;
using InsireBot.Localization.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace InsireBot
{
    public class MediaItemViewModel : TrackingBaseViewModel<Data.MediaItem>, IMediaItem, ISequence, IIdentifier, IValidatableTrackingObject, IValidatableObject
    {
        private int _id;
        public int ID
        {
            get { return _id; }
            private set { SetValue(ref _id, value); }
        }

        private int _sequence;
        public int Sequence
        {
            get { return _sequence; }
            set { SetValue(ref _sequence, value); }
        }

        private string _title;
        public string Title
        {
            get { return _title; }
            private set { SetValue(ref _title, value); }
        }

        private TimeSpan _duration;
        public TimeSpan Duration
        {
            get { return _duration; }
            private set { SetValue(ref _duration, value); }
        }

        private PrivacyStatus _privacyStatus;
        public PrivacyStatus PrivacyStatus
        {
            get { return _privacyStatus; }
            private set { SetValue(ref _privacyStatus, value); }
        }

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

        public MediaItemViewModel(Data.MediaItem model) : base(model)
        {
        }

        protected override void InitializeComplexProperties(Data.MediaItem model)
        {
            ID = model.Id;
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
                yield return new ValidationResult($"{nameof(Location)} {Resources.IsRequired}",  new[] { nameof(Location) });
        }
    }
}
