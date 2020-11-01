namespace Maple.Domain
{
    public interface IAudioDeviceTypeModel : IEntity<int>
    {
        DeviceType DeviceType { get; }
    }
}
