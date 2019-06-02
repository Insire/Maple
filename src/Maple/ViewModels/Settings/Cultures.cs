using System.Threading;
using System.Threading.Tasks;
using Maple.Domain;

namespace Maple
{
    public sealed class Cultures : MapleBusinessViewModelListBase<Culture>
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
