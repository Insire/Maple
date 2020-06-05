using Maple.Domain;
using MvvmScarletToolkit.Observables;

namespace Maple
{
    public sealed class MediaPlayers : ViewModelListBase<MediaPlayer>
    {
        private readonly MediaPlayerFactory _mediaPlayerFactory;

        private MediaPlayer _main;
        public MediaPlayer Main
        {
            get { return _main; }
            private set { SetValue(ref _main, value); }
        }

        public MediaPlayers(IMapleCommandBuilder commandBuilder, MediaPlayerFactory mediaPlayerFactory)
            : base(commandBuilder)
        {
            _mediaPlayerFactory = mediaPlayerFactory ?? throw new System.ArgumentNullException(nameof(mediaPlayerFactory));
        }

        internal CreateMediaPlayerViewModel Create()
        {
            return _mediaPlayerFactory.Create();
        }

        internal CreateMediaPlayerViewModel Create(MediaPlayer mediaPlayer)
        {
            return _mediaPlayerFactory.Create(mediaPlayer);
        }
    }
}
