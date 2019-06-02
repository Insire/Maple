using System.Threading;
using System.Threading.Tasks;
using Maple.Domain;

namespace Maple
{
    public sealed class MaplePlayer : BasePlayer
    {
        public MaplePlayer(IMapleCommandBuilder commandBuilder)
            : base(commandBuilder)
        {
        }

        public override bool IsPlaying { get; }
        public override int Volume { get; set; }
        public override int VolumeMax { get; }
        public override int VolumeMin { get; }

        public override bool CanPlay(IMediaItem item)
        {
            return false;
        }

        public override void Pause()
        {
        }

        public override bool Play(IMediaItem item)
        {
            return false;
        }

        public override void Stop()
        {
        }

        protected override Task RefreshInternal(CancellationToken token)
        {
            return Task.CompletedTask;
        }

        protected override Task UnloadInternal(CancellationToken token)
        {
            return Task.CompletedTask;
        }
    }
}
