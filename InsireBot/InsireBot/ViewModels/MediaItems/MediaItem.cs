using FluentValidation;
using Maple.Core;
using Maple.Data;
using System;
using System.Diagnostics;

namespace Maple
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="Maple.Core.BaseViewModel{Maple.Data.MediaItem}" />
    /// <seealso cref="Maple.Core.IMediaItem" />
    [DebuggerDisplay("{Title}, {Sequence} {Location}")]
    public class MediaItem : ValidableBaseDataViewModel<MediaItem, Data.MediaItem>, IMediaItem
    {
        private readonly IPlaylistContext _context;

        public bool IsNew => Model.IsNew;
        public bool IsDeleted => Model.IsDeleted;

        private int _id;
        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id
        {
            get { return _id; }
            private set { SetValue(ref _id, value, OnChanged: () => Model.Id = value); }
        }

        private int _sequence;
        /// <summary>
        /// Gets or sets the sequence.
        /// </summary>
        /// <value>
        /// The sequence.
        /// </value>
        public int Sequence
        {
            get { return _sequence; }
            set { SetValue(ref _sequence, value, OnChanged: () => Model.Sequence = value); }
        }

        private int _playlistId;
        /// <summary>
        /// Gets or sets the playlist identifier.
        /// </summary>
        /// <value>
        /// The playlist identifier.
        /// </value>
        public int PlaylistId
        {
            get { return _playlistId; }
            set { SetValue(ref _playlistId, value, OnChanged: () => Model.PlaylistId = value); }
        }

        private TimeSpan _duration;
        /// <summary>
        /// Gets the duration.
        /// </summary>
        /// <value>
        /// The duration.
        /// </value>
        public TimeSpan Duration
        {
            get { return _duration; }
            private set { SetValue(ref _duration, value, OnChanged: () => Model.Duration = value.Ticks); }
        }

        private PrivacyStatus _privacyStatus;
        /// <summary>
        /// Gets the privacy status.
        /// </summary>
        /// <value>
        /// The privacy status.
        /// </value>
        public PrivacyStatus PrivacyStatus
        {
            get { return _privacyStatus; }
            private set { SetValue(ref _privacyStatus, value, OnChanged: () => Model.PrivacyStatus = (int)value); }
        }

        private string _title;
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string Title
        {
            get { return _title; }
            set { SetValue(ref _title, value, OnChanged: () => Model.Title = value); }
        }

        private string _description;
        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description
        {
            get { return _description; }
            set { SetValue(ref _description, value, OnChanged: () => Model.Description = value); }
        }

        private string _location;
        /// <summary>
        /// Gets the location.
        /// </summary>
        /// <value>
        /// The location.
        /// </value>
        public string Location
        {
            get { return _location; }
            private set { SetValue(ref _location, value, OnChanged: () => Model.Location = value); }
        }

        private bool _isSelected;
        /// <summary>
        /// Gets or sets a value indicating whether this instance is selected.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is selected; otherwise, <c>false</c>.
        /// </value>
        public bool IsSelected
        {
            get { return _isSelected; }
            set { SetValue(ref _isSelected, value); }
        }

        private string _createdBy;
        public string CreatedBy
        {
            get { return _createdBy; }
            set { SetValue(ref _createdBy, value, OnChanged: () => Model.CreatedBy = value); }
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
            set { SetValue(ref _updatedOn, value, OnChanged: () => Model.CreatedOn = value); }
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
        public MediaItem(Data.MediaItem model, IValidator<MediaItem> validator, IPlaylistContext context)
            : base(model, validator)
        {
            _id = model.Id;
            _playlistId = model.PlaylistId;
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

            _context = context ?? throw new ArgumentNullException(nameof(context));

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
