using Maple.Domain;
using MvvmScarletToolkit.Observables;
using System;

namespace Maple
{
    public sealed class AudioDeviceType : ObservableObject, IAudioDeviceTypeModel
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

        private DeviceType _deviceType;
        public DeviceType DeviceType
        {
            get { return _deviceType; }
            private set { SetValue(ref _deviceType, value); }
        }

        public AudioDeviceType(AudioDeviceTypeModel model)
        {
            Id = model.Id;
            Name = model.Name;
            Sequence = model.Sequence;

            DeviceType = model.DeviceType;

            CreatedBy = model.CreatedBy;
            CreatedOn = model.CreatedOn;
            UpdatedBy = model.UpdatedBy;
            UpdatedOn = model.UpdatedOn;
        }
    }
}
