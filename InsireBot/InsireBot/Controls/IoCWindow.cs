using MahApps.Metro.Controls;

namespace InsireBot
{
    public class IoCWindow : ConfigurableWindow, IIocFrameworkElement
    {
        private IConfigurableWindowSettings _settings;

        public ITranslationManager TranslationManager { get; private set; }

        public IoCWindow(ITranslationManager container) : base()
        {
            TranslationManager = container;
        }

        protected override IConfigurableWindowSettings CreateSettings()
        {
            return _settings = _settings ?? new ShellSettings(this);
        }
    }
}
