using Maple.Core;
using System;
using System.IO;

namespace Maple
{
    public class MediaItemViewModel : BaseViewModel<Data.MediaItem>, IMediaItem, ISequence, IIdentifier
    {
        private int _id;
        public int Id
        {
            get { return _id; }
            private set { SetValue(ref _id, value, Changed: () => Model.Id = value); }
        }

        private int _sequence;
        public int Sequence
        {
            get { return _sequence; }
            set { SetValue(ref _sequence, value, Changed: () => Model.Sequence = value); }
        }


        private int _playlistId;
        public int PlaylistId
        {
            get { return _playlistId; }
            set { SetValue(ref _playlistId, value, Changed: () => Model.PlaylistId = value); }
        }

        private TimeSpan _duration;
        public TimeSpan Duration
        {
            get { return _duration; }
            private set { SetValue(ref _duration, value, Changed: () => Model.Duration = value.Ticks); }
        }

        private PrivacyStatus _privacyStatus;
        public PrivacyStatus PrivacyStatus
        {
            get { return _privacyStatus; }
            private set { SetValue(ref _privacyStatus, value, Changed: () => Model.PrivacyStatus = (int)value); }
        }

        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetValue(ref _title, value, Changed: () => Model.Title = value); }
        }

        private string _description;
        public string Description
        {
            get { return _description; }
            set { SetValue(ref _description, value, Changed: () => Model.Description = value); }
        }

        private string _location;
        public string Location
        {
            get { return _location; }
            private set { SetValue(ref _location, value, Changed: () => Model.Location = value); }
        }

        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set { SetValue(ref _isSelected, value); }
        }

        public bool IsFile => File.Exists(Location);

        public MediaItemViewModel(Data.MediaItem model) : base(model)
        {
            Id = model.Id;
            PlaylistId = model.PlaylistId;
            Location = model.Location;
            Description = model.Description;
            Title = model.Title;
            Sequence = model.Sequence;
            Duration = TimeSpan.FromTicks(model.Duration);
            PrivacyStatus = (PrivacyStatus)model.PrivacyStatus;
        }

        public override string ToString()
        {
            var result = Title == string.Empty ? Location : Title;
            return result;
        }

        //public override IEnumerable<ValidationResult> Validate(ValidationContext context)
        //{
        //    if (string.IsNullOrWhiteSpace(Title))
        //        yield return new ValidationResult($"{nameof(Title)} {Resources.IsRequired}", new[] { nameof(Title) });

        //    if (string.IsNullOrWhiteSpace(Location))
        //        yield return new ValidationResult($"{nameof(Location)} {Resources.IsRequired}", new[] { nameof(Location) });
        //}
    }
}
