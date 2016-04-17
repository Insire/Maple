using GalaSoft.MvvmLight.Messaging;

using InsireBotCore;

namespace InsireBot.ViewModel
{
    /// <summary>
    /// A ViewModel to manage Playlists
    /// </summary>
    public class PlaylistsViewModel : BotViewModelBase<Playlist>
    {
        public PlaylistsViewModel(IDataService dataService) : base(dataService)
        {
            if (IsInDesignMode)
            {
                var item = new Playlist("Music", "PL5LmATNaGcQxknuhgb_BkCKKIvcZro7iR");
                Items.Add(item);

                item = new Playlist("Test", "PL5LmATNaGcQxknuhgb_BkCKKIvcZro7iR");
                Items.Add(item);
            }
            else
            {
                // Code runs "for real"
                var item = new Playlist("Music", "PL5LmATNaGcQxknuhgb_BkCKKIvcZro7iR");
                Items.Add(item);

                item = new Playlist("Test", "PL5LmATNaGcQxknuhgb_BkCKKIvcZro7iR");
                Items.Add(item);

                Messenger.Default.Register<Playlist>(this, (playlist) =>
                {
                    Items.Add(playlist);
                });
            }
        }
    }
}
