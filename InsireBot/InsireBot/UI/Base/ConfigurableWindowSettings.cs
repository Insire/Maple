using Maple.Core;
using System.Configuration;
using System.Windows;

namespace Maple
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="Maple.Core.IConfigurableWindowSettings" />
    public abstract class ConfigurableWindowSettings : IConfigurableWindowSettings
    {
        private readonly ApplicationSettingsBase _settings;

        private readonly string _isFirstRunProp;
        private readonly string _windowLocationProp;
        private readonly string _windowSizeProp;
        private readonly string _windowStateProp;

        /// <summary>
        /// Returns true if the application has never
        /// been run before by the current user.  If
        /// this returns true, the Window's initial
        /// location is determined by the operating
        /// system, not the WindowLocation property.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is first run; otherwise, <c>false</c>.
        /// </value>
        public bool IsFirstRun
        {
            get { return GetValue<bool>(_isFirstRunProp); }
            protected set { SetValue(_isFirstRunProp, value); }
        }

        /// <summary>
        /// Gets/sets the Window's desktop coordinate.
        /// </summary>
        /// <value>
        /// The window location.
        /// </value>
        public Point WindowLocation
        {
            get { return GetValue<Point>(_windowLocationProp); }
            set { SetValue(_windowLocationProp, value); }
        }

        /// <summary>
        /// Gets/sets the size of the Window.
        /// </summary>
        /// <value>
        /// The size of the window.
        /// </value>
        public Size WindowSize
        {
            get { return GetValue<Size>(_windowSizeProp); }
            set { SetValue(_windowSizeProp, value); }
        }

        /// <summary>
        /// Gets/sets the WindowState of the Window.
        /// </summary>
        /// <value>
        /// The state of the window.
        /// </value>
        public WindowState WindowState
        {
            get { return GetValue<WindowState>(_windowStateProp); }
            set { SetValue(_windowStateProp, value); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurableWindowSettings"/> class.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="isFirstRunProp">The is first run property.</param>
        /// <param name="windowLocationProp">The window location property.</param>
        /// <param name="windowSizeProp">The window size property.</param>
        /// <param name="windowStateProp">The window state property.</param>
        public ConfigurableWindowSettings(ApplicationSettingsBase settings, string isFirstRunProp, string windowLocationProp, string windowSizeProp, string windowStateProp)
        {
            _settings = settings;

            _isFirstRunProp = isFirstRunProp;
            _windowLocationProp = windowLocationProp;
            _windowSizeProp = windowSizeProp;
            _windowStateProp = windowStateProp;
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propName">Name of the property.</param>
        /// <returns></returns>
        protected T GetValue<T>(string propName)
        {
            return (T)_settings[propName];
        }

        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <param name="propName">Name of the property.</param>
        /// <param name="value">The value.</param>
        protected void SetValue(string propName, object value)
        {
            _settings[propName] = value;
            _settings.Save();
        }
    }
}
