using Maple.Domain;
using MvvmScarletToolkit.Observables;
using System;

namespace Maple
{
    public abstract class AudioDevice : ObservableObject, IAudioDevice
    {
        private string _osId;
        public string OsId
        {
            get { return _osId; }
            private set { SetValue(ref _osId, value); }
        }

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

        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set { SetValue(ref _isSelected, value); }
        }

        private int _audioDeviceTypeId;
        public int AudioDeviceTypeId
        {
            get { return _audioDeviceTypeId; }
            protected set { SetValue(ref _audioDeviceTypeId, value); }
        }

        private AudioDeviceType _audioDeviceType;
        public AudioDeviceType AudioDeviceType
        {
            get { return _audioDeviceType; }
            private set { SetValue(ref _audioDeviceType, value); }
        }

        private string _createdBy;
        public string CreatedBy
        {
            get { return _createdBy; }
            private set { SetValue(ref _createdBy, value); }
        }

        private string _updatedBy;
        public string UpdatedBy
        {
            get { return _updatedBy; }
            private set { SetValue(ref _updatedBy, value); }
        }

        private DateTime _updatedOn;
        public DateTime UpdatedOn
        {
            get { return _updatedOn; }
            private set { SetValue(ref _updatedOn, value); }
        }

        private DateTime _createdOn;
        public DateTime CreatedOn
        {
            get { return _createdOn; }
            private set { SetValue(ref _createdOn, value); }
        }

        private bool _isDeleted;
        public bool IsDeleted
        {
            get { return _isDeleted; }
            private set { SetValue(ref _isDeleted, value); }
        }

        protected AudioDevice(string osIdentifier)
        {
            if (string.IsNullOrWhiteSpace(osIdentifier))
            {
                throw new ArgumentException("Can't be Null or WhiteSpace", nameof(osIdentifier));
            }

            OsId = osIdentifier;

            var now = DateTime.Now;

            CreatedBy = Environment.UserName;
            CreatedOn = now;
            UpdatedBy = Environment.UserName;
            UpdatedOn = now;
        }

        public void UpdateFromModel(AudioDeviceModel model, AudioDeviceType audioDeviceType)
        {
            Update(model);

            AudioDeviceType = audioDeviceType;
        }

        public void Update(IAudioDevice model)
        {
            Id = model.Id;
            Name = model.Name;
            Sequence = model.Sequence;

            AudioDeviceTypeId = model.AudioDeviceTypeId;

            CreatedBy = model.CreatedBy;
            CreatedOn = model.CreatedOn;
            UpdatedBy = model.UpdatedBy;
            UpdatedOn = model.UpdatedOn;
        }
    }
}
