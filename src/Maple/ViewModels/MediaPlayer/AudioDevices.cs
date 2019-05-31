using System;
using System.Threading;
using System.Threading.Tasks;
using Maple.Core;
using Maple.Domain;

namespace Maple
{
    public class AudioDevices : MapleBusinessViewModelListBase<IAudioDevice>
    {
        public AudioDevices(IMapleCommandBuilder commandBuilder)
            : base(commandBuilder)
        {
        }

        protected override Task RefreshInternal(CancellationToken token)
        {
            throw new NotImplementedException();
        }
    }
}
