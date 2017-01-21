using System.Configuration;
using System.Windows;

namespace InsireBot
{
    public class ConfigurableWindowSettings : IConfigurableWindowSettings
    {
        private readonly ApplicationSettingsBase _settings;

        private readonly string _isFirstRunProp;
        private readonly string _windowLocationProp;
        private readonly string _windowSizeProp;
        private readonly string _windowStateProp;

        public bool IsFirstRun
        {
            get { return GetValue<bool>(_isFirstRunProp); }
            protected set { SetValue(_isFirstRunProp, value); }
        }

        public Point WindowLocation
        {
            get { return GetValue<Point>(_windowLocationProp); }
            set { SetValue(_windowLocationProp, value); }
        }

        public Size WindowSize
        {
            get { return GetValue<Size>(_windowSizeProp); }
            set { SetValue(_windowSizeProp, value); }
        }

        public WindowState WindowState
        {
            get { return GetValue<WindowState>(_windowStateProp); }
            set { SetValue(_windowStateProp, value); }
        }

        public ConfigurableWindowSettings(ApplicationSettingsBase settings, string isFirstRunProp, string windowLocationProp, string windowSizeProp, string windowStateProp)
        {
            _settings = settings;

            _isFirstRunProp = isFirstRunProp;
            _windowLocationProp = windowLocationProp;
            _windowSizeProp = windowSizeProp;
            _windowStateProp = windowStateProp;
        }

        protected T GetValue<T>(string propName)
        {
            return (T)_settings[propName];
        }

        protected void SetValue(string propName, object value)
        {
            _settings[propName] = value;
            _settings.Save();
        }
    }
}
