using NAudio.Wave;

namespace Maple
{
    internal sealed class AsioDevice : BaseDevice
    {
        public AsioDevice(AsioOut device)
        {
            Name = device.DriverName;
        }
    }
}
