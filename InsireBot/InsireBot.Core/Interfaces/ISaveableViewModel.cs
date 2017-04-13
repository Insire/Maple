using System.Windows.Input;

namespace Maple.Core
{
    /// <summary>
    /// 
    /// </summary>
    public interface ISaveableViewModel
    {
        /// <summary>
        /// Gets the save command.
        /// </summary>
        /// <value>
        /// The save command.
        /// </value>
        ICommand SaveCommand { get; }
    }
}
