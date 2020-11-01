namespace Maple.Domain
{
    public interface IAudioDevice : IEntity<int>
    {
        int AudioDeviceTypeId { get; }
        string OsId { get; }
    }
}
