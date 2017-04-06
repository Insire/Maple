using Maple.Core;
using System;
using System.IO;

namespace Maple
{
    public class MediaItem : BaseViewModel<Data.MediaItem>, IMediaItem
    {
        public bool IsNew => Model.IsNew;
        public bool IsDeleted => Model.IsDeleted;

        private int _id;
        public int Id
        {
            get { return _id; }
            private set { SetValue(ref _id, value, OnChanged: () => Model.Id = value); }
        }

        private int _sequence;
        public int Sequence
        {
            get { return _sequence; }
            set { SetValue(ref _sequence, value, OnChanged: () => Model.Sequence = value); }
        }


        private int _playlistId;
        public int PlaylistId
        {
            get { return _playlistId; }
            set { SetValue(ref _playlistId, value, OnChanged: () => Model.PlaylistId = value); }
        }

        private TimeSpan _duration;
        public TimeSpan Duration
        {
            get { return _duration; }
            private set { SetValue(ref _duration, value, OnChanged: () => Model.Duration = value.Ticks); }
        }

        private PrivacyStatus _privacyStatus;
        public PrivacyStatus PrivacyStatus
        {
            get { return _privacyStatus; }
            private set { SetValue(ref _privacyStatus, value, OnChanged: () => Model.PrivacyStatus = (int)value); }
        }

        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetValue(ref _title, value, OnChanged: () => Model.Title = value); }
        }

        private string _description;
        public string Description
        {
            get { return _description; }
            set { SetValue(ref _description, value, OnChanged: () => Model.Description = value); }
        }

        private string _location;
        public string Location
        {
            get { return _location; }
            private set { SetValue(ref _location, value, OnChanged: () => Model.Location = value); }
        }

        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set { SetValue(ref _isSelected, value); }
        }

        public bool IsFile => File.Exists(Location);

        public MediaItem(Data.MediaItem model)
            : base(model)
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

        private void IntializeValidation()
        {
            AddRule(Title, new NotNullOrEmptyRule(nameof(Title)));
            AddRule(Description, new NotNullOrEmptyRule(nameof(Description)));
            AddRule(Location, new NotNullOrEmptyRule(nameof(Location)));
        }
    }
}
