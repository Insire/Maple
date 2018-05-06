using System;
using System.Diagnostics;
using FluentValidation;
using Maple.Domain;

namespace Maple.Core
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="Maple.Core.BaseViewModel{Maple.Data.MediaItem}" />
    /// <seealso cref="Maple.Core.IMediaItem" />
    [DebuggerDisplay("{Title}, {Sequence} {Location}")]
    public class MediaItem : ValidableBaseDataViewModel<MediaItem, MediaItemModel, int>, IMediaItem
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
            _mediaItemType = (MediaItemType)model.MediaItemType;

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
