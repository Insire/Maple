using GalaSoft.MvvmLight.Messaging;

using InsireBotCore;

namespace InsireBot.ViewModel
{
    /// <summary>
    /// BO ViewModel to manage Playlists
    /// </summary>
    public class PlaylistsViewModel : BusinessViewModelBase<Playlist<MediaItem>>
    {
        public PlaylistsViewModel(IDataService dataService) : base(dataService)
        {
            if (IsInDesignMode)
            {
                var item = new Playlist<MediaItem>("Music", "PL5LmATNaGcQxknuhgb_BkCKKIvcZro7iR");
                Items.Add(item);

                item = new Playlist<MediaItem>("Test", "PL5LmATNaGcQxknuhgb_BkCKKIvcZro7iR");
                Items.Add(item);
            }
            else
            {
                var item = new Playlist<MediaItem>("Music", "PL5LmATNaGcQxknuhgb_BkCKKIvcZro7iR");
                Items.Add(item);

                item = new Playlist<MediaItem>("Test", "PL5LmATNaGcQxknuhgb_BkCKKIvcZro7iR");
                Items.Add(item);

                Messenger.Default.Register<Playlist<MediaItem>>(this, (playlist) =>
                {
                    Items.Add(playlist);
                });
            }
        }
    }
}
