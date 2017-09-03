using System.Threading.Tasks;

namespace Maple.Interfaces
{
    public interface IRefreshable
    {
        void Save();
        Task LoadAsync();
    }
}
