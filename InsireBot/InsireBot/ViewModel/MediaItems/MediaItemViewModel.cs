using InsireBot.Core;
using System;
using System.ComponentModel.DataAnnotations;

namespace InsireBot
{
    public class MediaItemViewModel : TrackingBaseViewModel<Core.MediaItem>, IMediaItem, ISequence, IIdentifier, IValidatableTrackingObject, IValidatableObject
    {
        private Guid _id;
        public Guid ID
        {
            get { return _id; }
            private set { SetValue(ref _id, value); }
        }

        private int _sequence;
        /// <summary>
        /// An Index managed by the collection this <see cref="MediaItemViewModel"/> is part of
        /// </summary>
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

        private bool _isRestricted;
        public bool IsRestricted
        {
            get { return _isRestricted; }
            private set { SetValue(ref _isRestricted, value); }
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

        private bool _isLocalFile;
        public bool IsLocalFile
        {
            get { return _isLocalFile; }
            private set { SetValue(ref _isLocalFile, value); }
        }

        public MediaItemViewModel(MediaItem model) : base(model)
        {
        }

        public override string ToString()
        {
            var result = Title == string.Empty ? Location : Title;
            return result;
        }
    }
}
