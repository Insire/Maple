using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Maple.Domain;
using MvvmScarletToolkit;
using MvvmScarletToolkit.Observables;

namespace Maple
{
    public sealed class Cultures : MapleBusinessViewModelListBase<Culture>
    {
        public Cultures(IMapleCommandBuilder commandBuilder)
                : base(commandBuilder)
        {
            Messenger.Subscribe<ViewModelListBaseSelectionChanged<Culture>>((p) => LocalizationService.CurrentLanguage = p.Content.Model);
        }

        protected override async Task RefreshInternal(CancellationToken token)
        {
            await Task.WhenAll(LocalizationService.Languages.ForEachAsync(p => Add(new Culture(CommandBuilder, p))));
            await Dispatcher.Invoke(() => SelectedItem = Items.FirstOrDefault(p => p.Model == LocalizationService.CurrentLanguage));
        }
    }
}
