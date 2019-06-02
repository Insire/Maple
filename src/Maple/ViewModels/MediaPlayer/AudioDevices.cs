using System.Threading;
using System.Threading.Tasks;
using Maple.Domain;

namespace Maple
{
    public class AudioDevices : MapleBusinessViewModelListBase<IAudioDevice>
    {
        public AudioDevices(IMapleCommandBuilder commandBuilder)
            : base(commandBuilder)
        {
            _items.Add(new AudioDevice()
            {
                Channels = 5,
                IsSelected = true,
                Name = "Test",
                Sequence = 1,
            });

            _items.Add(new AudioDevice()
            {
                Channels = 2,
                IsSelected = false,
                Name = "Test 2",
                Sequence = 1,
            });
        }

        protected override Task RefreshInternal(CancellationToken token)
        {
            return Task.CompletedTask;
        }
    }
}
