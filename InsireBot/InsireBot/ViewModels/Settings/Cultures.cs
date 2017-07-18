using Maple.Core;
using Maple.Localization.Properties;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Maple
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="Maple.Core.ObservableObject" />
    /// <seealso cref="Maple.Core.ILoadableViewModel" />
    /// <seealso cref="Maple.Core.ISaveableViewModel" />
    public class Cultures : BaseListViewModel<Culture>, ICultureViewModel
    {
        private readonly ILocalizationService _manager;
        private readonly ILoggingService _log;

        /// <summary>
        /// Gets the refresh command.
        /// </summary>
        /// <value>
        /// The refresh command.
        /// </value>
        public ICommand RefreshCommand => new RelayCommand(Load);
        /// <summary>
        /// Gets the load command.
        /// </summary>
        /// <value>
        /// The load command.
        /// </value>
        public ICommand LoadCommand => new RelayCommand(Load, () => !IsLoaded);
        /// <summary>
        /// Gets the save command.
        /// </summary>
        /// <value>
        /// The save command.
        /// </value>
        public ICommand SaveCommand => new RelayCommand(Save);

        /// <summary>
        /// Initializes a new instance of the <see cref="CultureViewModel"/> class.
        /// </summary>
        /// <param name="manager">The manager.</param>
        public Cultures(ViewModelServiceContainer container)
            : base(container.Messenger)
        {
            _log = container.Log;
            _manager = container.LocalizationService;
        }

        private void SyncCulture()
        {
            _manager.CurrentLanguage = SelectedItem.Model;
        }

        /// <summary>
        /// Saves this instance.
        /// </summary>
        public void Save()
        {
            _log.Info($"{Resources.Saving} {Resources.Options}");
            _manager.Save();
        }

        /// <summary>
        /// Loads this instance.
        /// </summary>
        public void Load()
        {
            _log.Info($"{Resources.Loading} {Resources.Options}");
            _manager.Load();

            Initialise();

            IsLoaded = true;
            _messenger.Publish(new LoadedMessage(this, this));
        }

        public async Task SaveAsync()
        {
            _log.Info($"{Resources.Saving} {Resources.Options}");
            await _manager.SaveAsync();
        }

        public async Task LoadAsync()
        {
            _log.Info($"{Resources.Loading} {Resources.Options}");
            await _manager.LoadAsync();

            Initialise();

            IsLoaded = true;
            _messenger.Publish(new LoadedMessage(this, this));
        }

        private void Initialise()
        {
            Items.AddRange(_manager.Languages.Select(p => new Culture(p, _messenger)).ToList());
            SelectedItem = Items.FirstOrDefault(p => p.Model.LCID == Core.Properties.Settings.Default.StartUpCulture.LCID) ?? Items.First(p => p.Model.TwoLetterISOLanguageName == "en");
        }
    }
}
