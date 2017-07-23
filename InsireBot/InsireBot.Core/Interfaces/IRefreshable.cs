using System.Threading.Tasks;

namespace Maple.Core
{
    public interface IRefreshable
    {
        void Save();
        Task LoadAsync();
    }
}
