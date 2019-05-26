using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Maple.Core;
using Maple.Domain;
using Maple.Localization.Properties;

namespace Maple
{
    public sealed class Cultures : MapleBusinessViewModelListBase<Culture>, ICultureViewModel
    {
        public ICommand SaveCommand { get; }

        public Cultures(IMapleCommandBuilder commandBuilder)
                : base(commandBuilder)
        {
            MessageTokens.Add(Messenger.Subscribe<ViewModelSelectionChangedMessage<Culture>>(UpdateCulture));
        }

        private void SyncCulture()
        {
            _manager.CurrentLanguage = SelectedItem.Model;
        }

        public Task Save()
        {
            Log.Info($"{Resources.Saving} {Resources.Options}");
            _manager.Save();

            _log.Info($"{Resources.Saving} {Resources.Options}");

            return Task.CompletedTask;
        }

        public async Task Load()
        {
            Log.Info($"{Resources.Loading} {Resources.Options}");
            await _manager.Load().ConfigureAwait(true);

            await AddRange(_manager.Languages.Select(p => new Culture(p, Messenger)).ToList());
            SelectedItem = Items.FirstOrDefault(p => p.Model.LCID == Core.Properties.Settings.Default.StartUpCulture.LCID) ?? Items.First(p => p.Model.TwoLetterISOLanguageName == "en");

            IsLoaded = true;
            Messenger.Publish(new LoadedMessage(this, this));
        }

        private void UpdateCulture(ViewModelSelectionChangedMessage<Culture> obj)
        {
            if (obj.Content?.Model != null)
                _manager.CurrentLanguage = obj.Content.Model;
        }
    }
}
