using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Maple.Domain;

namespace Maple
{
    [DebuggerDisplay("{Title}, {Sequence} {Location}")]
    public class MediaItem : MapleDomainViewModelBase<MediaItem, MediaItemModel>, IMediaItem
    {
        public bool IsNew => Model.IsNew;
        public bool IsDeleted => Model.IsDeleted;

        public int Id
        {
            get { return Model.Id; }
        }

        private int _sequence;
        public int Sequence
        {
            get { return _sequence; }
            set { SetValue(ref _sequence, value, OnChanged: () => Model.Sequence = value); }
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

        private MediaItemType _mediaItemType;
        public MediaItemType MediaItemType
        {
            get { return _mediaItemType; }
            set { SetValue(ref _mediaItemType, value, OnChanged: () => Model.MediaItemType = (int)value); }
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

        private string _createdBy;
        public string CreatedBy
        {
            get { return _createdBy; }
            private set { SetValue(ref _createdBy, value, OnChanged: () => Model.CreatedBy = value); }
        }

        private string _updatedBy;
        public string UpdatedBy
        {
            get { return _updatedBy; }
            set { SetValue(ref _updatedBy, value, OnChanged: () => Model.UpdatedBy = value); }
        }

        private DateTime _updatedOn;
        public DateTime UpdatedOn
        {
            get { return _updatedOn; }
            set { SetValue(ref _updatedOn, value, OnChanged: () => Model.UpdatedOn = value); }
        }

        private DateTime _createdOn;
        public DateTime CreatedOn
        {
            get { return _createdOn; }
            private set { SetValue(ref _createdOn, value, OnChanged: () => Model.CreatedOn = value); }
        }

        private Playlist _playlist;
        public Playlist Playlist
        {
            get { return _playlist; }
            set { SetValue(ref _playlist, value, OnChanged: () => Model.Playlist = value.Model); }
        }

        public MediaItem(IMapleCommandBuilder commandBuilder, IValidator<MediaItem> validator, MediaItemModel model)
            : base(commandBuilder, validator, model)
        {
        }

        public override string ToString()
        {
            return Title?.Length == 0 ? Location : Title;
        }

        protected override Task UnloadInternal(CancellationToken token)
        {
            _location = string.Empty;
            _description = string.Empty;
            _title = string.Empty;
            _sequence = -1;
            _duration = TimeSpan.FromTicks(0);
            _privacyStatus = PrivacyStatus.None;
            _mediaItemType = MediaItemType.None;
            _createdBy = string.Empty;
            _createdOn = DateTime.MinValue;
            _updatedBy = string.Empty;
            _updatedOn = DateTime.MinValue;

            OnPropertyChanged(string.Empty);

            return Task.CompletedTask;
        }

        protected override Task RefreshInternal(CancellationToken token)
        {
            _location = Model.Location;
            _description = Model.Description;
            _title = Model.Title;
            _sequence = Model.Sequence;
            _duration = TimeSpan.FromTicks(Model.Duration);
            _privacyStatus = (PrivacyStatus)Model.PrivacyStatus;
            _mediaItemType = (MediaItemType)Model.MediaItemType;
            _createdBy = Model.CreatedBy;
            _createdOn = Model.CreatedOn;
            _updatedBy = Model.UpdatedBy;
            _updatedOn = Model.UpdatedOn;

            OnPropertyChanged(string.Empty);

            return Task.CompletedTask;
        }
    }
}
