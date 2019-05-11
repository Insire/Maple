using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;

using Maple.Core;
using Maple.Domain;

namespace Maple
{
    [DebuggerDisplay("{Title}, {Sequence} {Location}")]
    public class MediaItem : ValidableBaseDataViewModel<MediaItem, MediaItemModel>, IMediaItem
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

        public bool IsFile => IOUtils.IsLocalFile(Location);

        public MediaItem(MediaItemModel model, IValidator<MediaItem> validator, IMessenger messenger)
            : base(model, validator, messenger)
        {
            _location = model.Location;
            _description = model.Description;
            _title = model.Title;
            _sequence = model.Sequence;
            _duration = TimeSpan.FromTicks(model.Duration);
            _privacyStatus = (PrivacyStatus)model.PrivacyStatus;
            _mediaItemType = (MediaItemType)model.MediaItemType;
            _createdBy = model.CreatedBy;
            _createdOn = model.CreatedOn;
            _updatedBy = model.UpdatedBy;
            _updatedOn = model.UpdatedOn;

            Validate();
        }

        public override string ToString()
        {
            return Title?.Length == 0 ? Location : Title;
        }

        protected override Task Load(CancellationToken token)
        {
            throw new NotImplementedException();
        }

        protected override Task Unload(CancellationToken token)
        {
            throw new NotImplementedException();
        }

        protected override Task Refresh(CancellationToken token)
        {
            throw new NotImplementedException();
        }
    }
}
