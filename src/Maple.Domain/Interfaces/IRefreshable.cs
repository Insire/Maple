using System.Threading.Tasks;

namespace Maple.Domain
{
    public interface ILoadAndSaveProvider
    {
        Task Save();
        Task LoadAsync();
    }
}
