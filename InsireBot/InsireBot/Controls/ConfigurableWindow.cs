using MahApps.Metro.Controls;
using System;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace InsireBot
{
    public abstract class ConfigurableWindow : MetroWindow
    {
        private bool _isLoaded;
        private readonly IConfigurableWindowSettings _settings;

        protected ConfigurableWindow()
        {
            _settings = CreateSettings();

            if (_settings == null)
                throw new ArgumentNullException(nameof(_settings), "Cannot be null.");

            Loaded += delegate { _isLoaded = true; };

            ApplySettings();
        }

        /// <summary>
        /// Derived classes must return the object which exposes 
        /// persisted window settings. This method is only invoked 
        /// once per Window, during construction.
        /// </summary>
        protected abstract IConfigurableWindowSettings CreateSettings();

        protected override void OnLocationChanged(EventArgs e)
        {
            base.OnLocationChanged(e);

            // We need to delay this call because we are 
            // notified of a location change before a 
            // window state change.  That causes a problem 
            // when maximizing the window because we record 
            // the maximized window's location, which is not 
            // something worth saving.
            Dispatcher.BeginInvoke(DispatcherPriority.Background, new ThreadStart(() =>
            {
                if (_isLoaded && WindowState == WindowState.Normal)
                {
                    var loc = new Point(Left, Top);
                    _settings.WindowLocation = loc;
                }
            }));
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo info)
        {
            base.OnRenderSizeChanged(info);

            if (_isLoaded && WindowState == WindowState.Normal)
            {
                _settings.WindowSize = RenderSize;
            }
        }

        protected override void OnStateChanged(EventArgs e)
        {
            base.OnStateChanged(e);

            if (_isLoaded)
            {
                // We don't want the Window to open in the 
                // minimized state, so ignore that value.
                if (WindowState != WindowState.Minimized)
                    _settings.WindowState = WindowState;
                else
                    _settings.WindowState = WindowState.Normal;
            }
        }

        void ApplySettings()
        {
            var size = _settings.WindowSize;
            Width = size.Width;
            Height = size.Height;

            var loccation = _settings.WindowLocation;

            // If the user's machine had two monitors but now only
            // has one, and the Window was previously on the other
            // monitor, we need to move the Window into view.
            bool outOfBounds = loccation.X <= -size.Width
                            || loccation.Y <= -size.Height
                            || SystemParameters.VirtualScreenWidth <= loccation.X
                            || SystemParameters.VirtualScreenHeight <= loccation.Y;

            if (_settings.IsFirstRun || outOfBounds)
                WindowStartupLocation = WindowStartupLocation.CenterScreen;
            else
            {
                WindowStartupLocation = WindowStartupLocation.Manual;

                Left = loccation.X;
                Top = loccation.Y;

                // We need to wait until the HWND window is initialized before
                // setting the state, to ensure that this works correctly on
                // a multi-monitor system.  Thanks to Andrew Smith for this fix.
                SourceInitialized += delegate
                {
                    WindowState = _settings.WindowState;
                };
            }
        }
    }
}
