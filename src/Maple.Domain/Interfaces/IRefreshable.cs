using System.Threading.Tasks;

namespace Maple.Domain
{
    public interface IRefreshable
    {
        void Save();
        Task LoadAsync();
    }
}
