using System;
using System.Diagnostics;
using Maple.Domain;
using MvvmScarletToolkit;
using MvvmScarletToolkit.Observables;

namespace Maple
{
    [DebuggerDisplay("MediaItem: {Sequence}, {Name}, {Location}")]
    public sealed class MediaItem : ViewModelBase, IMediaItem
    {
        private int _id;
        public int Id
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

        private string _name;
        public string Name
        {
            get { return _name; }
            set { SetValue(ref _name, value); }
        }

        private string _thumbnail;
        public string Thumbnail
        {
            get { return _thumbnail; }
            set { SetValue(ref _thumbnail, value); }
        }

        private string _location;
        public string Location
        {
            get { return _location; }
            set { SetValue(ref _location, value); }
        }

        private Playlist _playlist;
        public Playlist Playlist
        {
            get { return _playlist; }
            set { SetValue(ref _playlist, value); }
        }

        private TimeSpan _duration;
        public TimeSpan Duration
        {
            get { return _duration; }
            set { SetValue(ref _duration, value); }
        }

        private PrivacyStatus _privacyStatus;
        public PrivacyStatus PrivacyStatus
        {
            get { return _privacyStatus; }
            set { SetValue(ref _privacyStatus, value); }
        }

        private MediaItemType _mediaItemType;
        public MediaItemType MediaItemType
        {
            get { return _mediaItemType; }
            set { SetValue(ref _mediaItemType, value); }
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
            set { SetValue(ref _createdBy, value); }
        }

        private string _updatedBy;
        public string UpdatedBy
        {
            get { return _updatedBy; }
            set { SetValue(ref _updatedBy, value); }
        }

        private DateTime _updatedOn;
        public DateTime UpdatedOn
        {
            get { return _updatedOn; }
            set { SetValue(ref _updatedOn, value); }
        }

        private DateTime _createdOn;
        public DateTime CreatedOn
        {
            get { return _createdOn; }
            set { SetValue(ref _createdOn, value); }
        }

        private bool _isDeleted;
        public bool IsDeleted
        {
            get { return _isDeleted; }
            private set { SetValue(ref _isDeleted, value); }
        }

        public MediaItem(IScarletCommandBuilder commandBuilder)
            : base(commandBuilder)
        {
        }

        public MediaItem(MediaItem mediaItem)
            : this(mediaItem.CommandBuilder)
        {
            Id = mediaItem.Id;
            Name = mediaItem.Name;
            Sequence = mediaItem.Sequence;

            Location = mediaItem.Location;
            Thumbnail = mediaItem.Thumbnail;
            Duration = mediaItem.Duration;
            MediaItemType = mediaItem.MediaItemType;
            PrivacyStatus = mediaItem.PrivacyStatus;

            Playlist = mediaItem.Playlist;

            CreatedBy = mediaItem.CreatedBy;
            CreatedOn = mediaItem.CreatedOn;
            UpdatedBy = mediaItem.UpdatedBy;
            UpdatedOn = mediaItem.UpdatedOn;
        }

        public MediaItem(IScarletCommandBuilder commandBuilder, MediaItemModel mediaItem, Playlist playlist)
            : this(commandBuilder)
        {
            Id = mediaItem.Id;
            Name = mediaItem.Name;
            Sequence = mediaItem.Sequence;

            Location = mediaItem.Location;
            Thumbnail = mediaItem.Thumbnail;
            Duration = mediaItem.Duration;
            MediaItemType = mediaItem.MediaItemType;
            PrivacyStatus = mediaItem.PrivacyStatus;

            Playlist = playlist;

            CreatedBy = mediaItem.CreatedBy;
            CreatedOn = mediaItem.CreatedOn;
            UpdatedBy = mediaItem.UpdatedBy;
            UpdatedOn = mediaItem.UpdatedOn;
        }
    }
}
