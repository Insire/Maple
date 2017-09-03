using System.Windows.Input;

namespace Maple.Core
{
    public interface ISaveableViewModel
    {
        ICommand SaveCommand { get; }
    }
}
