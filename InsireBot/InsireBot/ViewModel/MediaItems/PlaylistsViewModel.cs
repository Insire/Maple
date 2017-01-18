using GalaSoft.MvvmLight.Messaging;

namespace InsireBot
{
    /// <summary>
    /// BO ViewModel to manage Playlists
    /// </summary>
    public class PlaylistsViewModel : BusinessViewModelBase<Playlist>
    {
        public PlaylistsViewModel(IDataService dataService) : base(dataService)
        {
            if (IsInDesignMode)
            {
                var item = new Playlist("Music", "https://www.youtube.com/playlist?list=PL5LmATNaGcQxknuhgb_BkCKKIvcZro7iR", 2);
                Items.Add(item);

                item = new Playlist("Test", "https://www.youtube.com/playlist?list=PL5LmATNaGcQxknuhgb_BkCKKIvcZro7iR", 5);
                Items.Add(item);
            }
            else
            {
                AddRange(_dataService.GetPlaylists());

                if (SelectedItem == null && Items.Count > 0)
                    SelectedItem = Items[0];

                Messenger.Default.Register<Playlist>(this, (playlist) =>
                {
                    Items.Add(playlist);
                });
            }
        }
    }
}
