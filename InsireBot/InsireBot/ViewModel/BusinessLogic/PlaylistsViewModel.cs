using GalaSoft.MvvmLight.Messaging;

using InsireBotCore;

namespace InsireBot
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
                var item = new Playlist<MediaItem>("Music", "https://www.youtube.com/playlist?list=PL5LmATNaGcQxknuhgb_BkCKKIvcZro7iR", 2);
                Items.Add(item);

                item = new Playlist<MediaItem>("Test", "https://www.youtube.com/playlist?list=PL5LmATNaGcQxknuhgb_BkCKKIvcZro7iR", 5);
                Items.Add(item);
            }
            else
            {
                var item = new Playlist<MediaItem>("Music", "https://www.youtube.com/playlist?list=PL5LmATNaGcQxknuhgb_BkCKKIvcZro7iR", 7);
                Items.Add(item);

                item = new Playlist<MediaItem>("Test", "https://www.youtube.com/playlist?list=PL5LmATNaGcQxknuhgb_BkCKKIvcZro7iR", 12);
                Items.Add(item);

                Messenger.Default.Register<Playlist<MediaItem>>(this, (playlist) =>
                {
                    Items.Add(playlist);
                });
            }
        }
    }
}
