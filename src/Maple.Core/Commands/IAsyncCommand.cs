using System.Threading.Tasks;
using System.Windows.Input;

namespace Maple.Core
{
    public interface IAsyncCommand : ICommand
    {
        Task ExecuteAsync(object parameter);
    }
}
