using InsireBot.Data;

namespace InsireBot
{
    public class MainMediaPlayerViewModel : MediaPlayerViewModel
    {
        private readonly ITranslationManager _manager;
        private readonly string _nameKey;

        public MainMediaPlayerViewModel(ITranslationManager manager, IMediaPlayer player, MediaPlayer mediaPlayer, string nameKey) : base(player, mediaPlayer)
        {
            _manager = manager;
            _nameKey = nameKey;

            _manager.PropertyChanged += (o, e) =>
              {
                  if (e.PropertyName == nameof(ITranslationManager.CurrentLanguage))
                      UpdateName();
              };

            UpdateName();
        }

        private void UpdateName()
        {
            Name = _manager.Translate(_nameKey);
        }
    }
}
