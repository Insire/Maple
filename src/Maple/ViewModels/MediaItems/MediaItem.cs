using System;
using System.Diagnostics;
using FluentValidation;
using Maple.Core;
using Maple.Domain;

namespace Maple
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="Maple.Core.BaseViewModel{Maple.Data.MediaItem}" />
    /// <seealso cref="Maple.Core.IMediaItem" />
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
            get { return _updatedOn; }
            private set { SetValue(ref _updatedOn, value, OnChanged: () => Model.CreatedOn = value); }
        }

        private Playlist _playlist;
        public Playlist Playlist
        {
            get { return _playlist; }
            set { SetValue(ref _playlist, value, OnChanged: () => Model.Playlist = value.Model); }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is file.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is file; otherwise, <c>false</c>.
        /// </value>
        public bool IsFile => IOUtils.IsLocalFile(Location);

        /// <summary>
        /// Initializes a new instance of the <see cref="MediaItem"/> class.
        /// </summary>
        /// <param name="model">The model.</param>
        public MediaItem(MediaItemModel model, IValidator<MediaItem> validator, IMessenger messenger)
            : base(model, validator, messenger)
        {
            _location = model.Location;
            _description = model.Description;
            _title = model.Title;
            _sequence = model.Sequence;
            _duration = TimeSpan.FromTicks(model.Duration);
            _privacyStatus = (PrivacyStatus)model.PrivacyStatus;
            _createdBy = model.CreatedBy;
            _createdOn = model.CreatedOn;
            _updatedBy = model.UpdatedBy;
            _updatedOn = model.UpdatedOn;

            Validate();
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            var result = Title == string.Empty ? Location : Title;
            return result;
        }
    }
}
