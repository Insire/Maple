using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Maple.Core;
using Maple.Domain;
using Maple.Localization.Properties;

namespace Maple
{
    public sealed class Cultures : MapleBusinessViewModelListBase<Culture>, ICultureViewModel
    {
        public Cultures(IMapleCommandBuilder commandBuilder)
                : base(commandBuilder)
        {
        }

        protected override async Task RefreshInternal(CancellationToken token)
        {
            // TODO
            //Add(Messenger.Subscribe<ViewModelSelectionChangedMessage<Culture>>(UpdateCulture));

            //Log.Info($"{Resources.Loading} {Resources.Options}");
            //await _manager.Load().ConfigureAwait(true);

            //await AddRange(LocalizationService.Languages.Select(p => new Culture(CommandBuilder, p)).ToList());
            //SelectedItem = Items.FirstOrDefault(p => p.Model.LCID == Core.Properties.Settings.Default.StartUpCulture.LCID) ?? Items.First(p => p.Model.TwoLetterISOLanguageName == "en");

            //IsLoaded = true;
            //Messenger.Publish(new LoadedMessage(this, this));
        }

        private void UpdateCulture(ViewModelSelectionChangedMessage<Culture> obj)
        {
            if (obj.Content?.Model != null)
                LocalizationService.CurrentLanguage = obj.Content.Model;
        }
    }
}
