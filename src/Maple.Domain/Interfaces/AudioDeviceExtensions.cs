namespace Maple.Domain
{
    public static class AudioDeviceExtensions
    {
        public static int GetKey(this IAudioDevice device)
        {
            var hCode = device.OsId.GetHashCode() ^ device.AudioDeviceTypeId;
            return hCode.GetHashCode();
        }

        public static AudioDeviceModel GetModel(this IAudioDevice instance)
        {
            return new AudioDeviceModel()
            {
                Id = instance.Id,
                Name = instance.Name,
                Sequence = instance.Sequence,

                OsId = instance.OsId,
                AudioDeviceTypeId = instance.AudioDeviceTypeId,

                CreatedBy = instance.CreatedBy,
                CreatedOn = instance.CreatedOn,
                UpdatedBy = instance.UpdatedBy,
                UpdatedOn = instance.UpdatedOn,
            };
        }
    }
}
