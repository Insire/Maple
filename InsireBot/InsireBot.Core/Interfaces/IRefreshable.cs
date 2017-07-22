using System.Threading.Tasks;

namespace Maple.Core
{
    public interface IRefreshable
    {
        Task SaveAsync();
        Task LoadAsync();
    }
}
