using Maple.Domain;
using MvvmScarletToolkit;

namespace Maple
{
    public sealed class DefaultPlaybackService : PlaybackServiceBase
    {
        public override bool IsPlaying => false;

        public override int Volume
        {
            get { return 0; }
            set
            {
            }
        }

        public override int VolumeMax => 1;

        public override int VolumeMin => 0;

        public DefaultPlaybackService(IScarletCommandBuilder commandBuilder)
            : base(commandBuilder)
        {
        }

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
    }
}
