using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Maple.Core;
using Maple.Interfaces;

namespace Maple
{
    public abstract class IoCWindow : ConfigurableWindow, IIocFrameworkElement
    {
        private IConfigurableWindowSettings _settings;
        private IMessenger _messenger;
        public ILocalizationService TranslationManager { get; private set; }

        public IoCWindow()
            : base()
        {
            if (Debugger.IsAttached)
                Debug.Fail($"The constructor without parameters of {nameof(IoCWindow)} exists only for compatibility reasons.");
        }

        public IoCWindow(ILocalizationService container, IMessenger messenger)
            : base()
        {
            TranslationManager = container ?? throw new ArgumentNullException(nameof(container), $"{nameof(container)} {Localization.Properties.Resources.IsRequired}");
            _messenger = messenger ?? throw new ArgumentNullException(nameof(messenger), $"{nameof(messenger)} {Localization.Properties.Resources.IsRequired}");

            _messenger.Subscribe<UiPrimaryColorChangedMessage>(PrimaryColorChanged);
        }

        /// <summary>
        /// Derived classes must return the object which exposes
        /// persisted window settings. This method is only invoked
        /// once per Window, during construction.
        /// </summary>
        /// <returns></returns>
        protected override IConfigurableWindowSettings CreateSettings()
        {
            return _settings = _settings ?? new ShellSettings(this);
        }

        private void PrimaryColorChanged(UiPrimaryColorChangedMessage e)
        {
            var data = string.Empty;
            if (PackIcon.TryGet(PackIconKind.ApplicationIcon, out data))
            {
                var geo = Geometry.Parse(data);
                Icon = SetImage(geo, e.Content);
            }
        }

        private BitmapSource SetImage(Geometry geo, Color color)
        {
            var canvas = new Canvas
            {
                Width = 36,
                Height = 36,
                Background = new SolidColorBrush(Colors.Transparent)
            };

            var path = new System.Windows.Shapes.Path()
            {
                Data = geo,
                Stretch = Stretch.Fill,
                Fill = new SolidColorBrush(color),
                Width = 36,
                Height = 36,
            };

            canvas.Children.Add(path);

            var size = new Size(36, 36);
            canvas.Measure(size);
            canvas.Arrange(new Rect(size));

            var rtb = new RenderTargetBitmap((int)size.Width, (int)size.Height, 96, 96, PixelFormats.Pbgra32);
            rtb.Render(canvas);

            var png = new PngBitmapEncoder();
            png.Frames.Add(BitmapFrame.Create(rtb));

            using (var memory = new MemoryStream())
            {
                png.Save(memory);
                memory.Position = 0;

                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();

                return bitmapImage;
            }
        }
    }
}
