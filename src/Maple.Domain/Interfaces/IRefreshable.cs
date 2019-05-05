using System.Threading.Tasks;

namespace Maple.Domain
{
    public interface IRefreshable
    {
        Task Save();

        Task Load();
    }
}
