using System.Windows;

namespace Maple
{
    public interface IConfigurableWindowSettings
    {
        /// <summary>
        /// Returns true if the application has never 
        /// been run before by the current user.  If
        /// this returns true, the Window's initial
        /// location is determined by the operating
        /// system, not the WindowLocation property.
        /// </summary>
        bool IsFirstRun { get; }

        /// <summary>
        /// Gets/sets the Window's desktop coordinate.
        /// </summary>
        Point WindowLocation { get; set; }

        /// <summary>
        /// Gets/sets the size of the Window.
        /// </summary>
        Size WindowSize { get; set; }

        /// <summary>
        /// Gets/sets the WindowState of the Window.
        /// </summary>
        WindowState WindowState { get; set; }
    }
}
