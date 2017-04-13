using System.Windows;

namespace Maple.Core
{
    /// <summary>
    /// 
    /// </summary>
    public interface IConfigurableWindowSettings
    {
        /// <summary>
        /// Returns true if the application has never
        /// been run before by the current user.  If
        /// this returns true, the Window's initial
        /// location is determined by the operating
        /// system, not the WindowLocation property.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is first run; otherwise, <c>false</c>.
        /// </value>
        bool IsFirstRun { get; }

        /// <summary>
        /// Gets/sets the Window's desktop coordinate.
        /// </summary>
        /// <value>
        /// The window location.
        /// </value>
        Point WindowLocation { get; set; }

        /// <summary>
        /// Gets/sets the size of the Window.
        /// </summary>
        /// <value>
        /// The size of the window.
        /// </value>
        Size WindowSize { get; set; }

        /// <summary>
        /// Gets/sets the WindowState of the Window.
        /// </summary>
        /// <value>
        /// The state of the window.
        /// </value>
        WindowState WindowState { get; set; }
    }
}
