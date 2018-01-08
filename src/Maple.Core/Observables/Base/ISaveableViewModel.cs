using System.Threading.Tasks;

namespace Maple.Core
{
    public interface ISaveableViewModel
    {
        IAsyncCommand SaveCommand { get; }

        Task SaveAsync();
    }
}
