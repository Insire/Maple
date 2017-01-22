using InsireBot.Core;
using InsireBot.Data;

namespace InsireBot
{
    /// <summary>
    /// BO ViewModel to manage Playlists
    /// </summary>
    public class LibraryViewModel
    {
        private IBotLog _log;
        private IPlaylistsRepository _repository;

        public LibraryViewModel(IBotLog log, IPlaylistsRepository repository)
        {
            _log = log;
            _repository = repository;
        }
    }
}
