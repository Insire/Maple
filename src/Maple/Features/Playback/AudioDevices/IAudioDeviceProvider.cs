using Maple.Domain;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace Maple
{
    public interface IAudioDeviceProvider
    {
        Task<ReadOnlyCollection<AudioDevice>> Get(DeviceType type, CancellationToken token);
    }
}
