using Maple.Core;
using Maple.Localization.Properties;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Maple
{
    public class Cultures : BaseListViewModel<Culture>, ICultureViewModel
    {
        private readonly ILocalizationService _manager;
        private readonly ILoggingService _log;

        public ICommand RefreshCommand => new AsyncRelayCommand(LoadAsync);
        public ICommand LoadCommand => new AsyncRelayCommand(LoadAsync, () => !IsLoaded);
        public ICommand SaveCommand => new RelayCommand(Save);

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

        public void Save()
        {
            _log.Info($"{Resources.Saving} {Resources.Options}");
            _manager.Save();
        }

        public Task SaveAsync()
        {
            return Task.Run(() =>
            {
                _log.Info($"{Resources.Saving} {Resources.Options}");
                _manager.Save();

            });
        }

        public async Task LoadAsync()
        {
            _log.Info($"{Resources.Loading} {Resources.Options}");
            await _manager.LoadAsync().ConfigureAwait(true);

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
