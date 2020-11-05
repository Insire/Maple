using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MvvmScarletToolkit;
using MvvmScarletToolkit.Abstractions;
using MvvmScarletToolkit.Observables;

namespace Maple
{
    public sealed class Cultures : BusinessViewModelListBase<Culture>
    {
        private readonly ILocalizationService _localizationService;

        public Cultures(IScarletCommandBuilder commandBuilder, ILocalizationService localizationService)
                : base(commandBuilder)
        {
            Messenger.Subscribe<ViewModelListBaseSelectionChanged<Culture>>((p) => _localizationService.CurrentLanguage = p.Content.Model);
            _localizationService = localizationService;
        }

        protected override async Task RefreshInternal(CancellationToken token)
        {
            await Task.WhenAll(_localizationService.Languages.ForEachAsync(p => Add(new Culture(CommandBuilder, p))));
            await Dispatcher.Invoke(() => SelectedItem = Items.FirstOrDefault(p => p.Model == _localizationService.CurrentLanguage));
        }
    }
}
