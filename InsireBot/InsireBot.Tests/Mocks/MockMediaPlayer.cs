using Maple.Core;
using System;

namespace Maple.Tests
{
    public class MockMediaPlayer : BasePlayer, IMediaPlayer
    {
        public override bool IsPlaying { get; }

        public override int Volume { get; set; }

        public override int VolumeMax { get; }

        public override int VolumeMin { get; }

        public override bool CanPlay(IMediaItem item)
        {
            return false;
        }

        public override void Dispose()
        {

        }

        public override void Pause()
        {

        }

        public override void Play(IMediaItem mediaItem)
        {

        }

        public override void Stop()
        {

        }
    }
}
