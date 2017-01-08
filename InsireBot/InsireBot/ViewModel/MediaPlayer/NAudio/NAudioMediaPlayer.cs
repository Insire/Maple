using System;

namespace InsireBot
{
    public class NAudioMediaPlayer : BasePlayer
    {
        public NAudioMediaPlayer(IDataService dataService) : base(dataService)
        {
        }

        public override bool CanPlay
        {
            get
            {
                return false;
            }
        }

        public override bool IsPlaying
        {
            get
            {
                return false;
            }
        }

        public override bool Silent
        {
            get
            {
                return false;
            }

            set
            {

            }
        }

        public override int Volume
        {
            get
            {
                return 0;
            }

            set
            {

            }
        }

        public sealed override void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Dispose(bool disposing)
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
