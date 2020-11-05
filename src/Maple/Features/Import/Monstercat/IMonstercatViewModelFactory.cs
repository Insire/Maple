using System.Threading;
using System.Threading.Tasks;

namespace Maple
{
    public interface IMonstercatViewModelFactory
    {
        Task<IMonstercatViewModel> Create(string input, CancellationToken token);
    }
}
