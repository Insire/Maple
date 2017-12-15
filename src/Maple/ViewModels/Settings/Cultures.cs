using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Maple.Core;
using Maple.Domain;
using Maple.Localization.Properties;

namespace Maple
{
    public class Cultures : BaseListViewModel<Culture>, ICultureViewModel
    {
        private readonly ILocalizationService _manager;
        private readonly ILoggingService _log;

        public ICommand RefreshCommand => AsyncCommand.Create(LoadAsync);
        public ICommand LoadCommand => AsyncCommand.Create(LoadAsync, () => !IsLoaded);
        public ICommand SaveCommand => new RelayCommand(Save);

        public Cultures(ViewModelServiceContainer container)
            : base(container.Messenger)
        {
            _log = container.Log;
            _manager = container.LocalizationService;

            MessageTokens.Add(Messenger.Subscribe<ViewModelSelectionChangedMessage<Culture>>(UpdateCulture));
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
            Messenger.Publish(new LoadedMessage(this, this));
        }

        private void Initialise()
        {
            AddRange(_manager.Languages.Select(p => new Culture(p, Messenger)).ToList());
            SelectedItem = Items.FirstOrDefault(p => p.Model.LCID == Core.Properties.Settings.Default.StartUpCulture.LCID) ?? Items.First(p => p.Model.TwoLetterISOLanguageName == "en");
        }

        private void UpdateCulture(ViewModelSelectionChangedMessage<Culture> obj)
        {
            if (obj.Content?.Model != null)
                _manager.CurrentLanguage = obj.Content.Model;
        }
    }
}
