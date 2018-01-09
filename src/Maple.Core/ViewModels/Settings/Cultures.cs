using System.Linq;
using System.Threading.Tasks;
using Maple.Domain;
using Maple.Localization.Properties;

namespace Maple.Core
{
    public class Cultures : BaseListViewModel<Culture>, ICultureViewModel
    {
        private readonly ILocalizationService _manager;
        private readonly ILoggingService _log;

        public IAsyncCommand RefreshCommand => AsyncCommand.Create(LoadAsync);
        public IAsyncCommand LoadCommand => AsyncCommand.Create(LoadAsync, () => !IsLoaded);
        public IAsyncCommand SaveCommand => AsyncCommand.Create(Save);

        private bool _isLoaded;
        /// <summary>
        /// Indicates whether the LoadCommand/ the Load Method has been executed yet
        /// </summary>
        public bool IsLoaded
        {
            get { return _isLoaded; }
            protected set
            {
                if (value)
                    SetValue(ref _isLoaded, value, OnChanged: () => Messenger.Publish(new LoadedMessage(this, this)));
            }
        }

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

        public Task Save()
        {
            _log.Info($"{Resources.Saving} {Resources.Options}");
            _manager.Save();

            return Task.CompletedTask;
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
