namespace Maple.Domain
{
    public static class AudioDeviceTypeExtensions
    {
        public static AudioDeviceTypeModel GetModel(this IAudioDeviceTypeModel instance)
        {
            return new AudioDeviceTypeModel()
            {
                Id = instance.Id,
                Name = instance.Name,
                Sequence = instance.Sequence,

                DeviceType = instance.DeviceType,

                CreatedBy = instance.CreatedBy,
                CreatedOn = instance.CreatedOn,
                UpdatedBy = instance.UpdatedBy,
                UpdatedOn = instance.UpdatedOn,
            };
        }
    }
}
