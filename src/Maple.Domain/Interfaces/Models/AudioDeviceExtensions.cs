namespace Maple.Domain
{
    public static class AudioDeviceExtensions
    {
        public static int GetKey(this IAudioDevice device)
        {
            var hCode = device.OsId.GetHashCode() ^ device.AudioDeviceTypeId;
            return hCode.GetHashCode();
        }

        public static AudioDeviceModel GetModel(this IAudioDevice device)
        {
            return new AudioDeviceModel()
            {
                Id = device.Id,
                Name = device.Name,
                Sequence = device.Sequence,

                OsId = device.OsId,
                AudioDeviceTypeId = device.AudioDeviceTypeId,

                CreatedBy = device.CreatedBy,
                CreatedOn = device.CreatedOn,
                UpdatedBy = device.UpdatedBy,
                UpdatedOn = device.UpdatedOn,
            };
        }
    }
}
