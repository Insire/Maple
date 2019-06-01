using System.Threading;
using System.Threading.Tasks;
using Maple.Core;
using Maple.Domain;

namespace Maple
{
    public sealed class Cultures : MapleBusinessViewModelListBase<Culture>, ICultureViewModel
    {
        public Cultures(IMapleCommandBuilder commandBuilder)
                : base(commandBuilder)
        {
        }

        protected override Task RefreshInternal(CancellationToken token)
        {
            return Task.CompletedTask;
        }
    }
}
