using Maple.Core;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Maple
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="Maple.ConfigurableWindow" />
    /// <seealso cref="Maple.Core.IIocFrameworkElement" />
    public abstract class IoCWindow : ConfigurableWindow, IIocFrameworkElement
    {
        private IConfigurableWindowSettings _settings;
        private IUIColorsViewModel _colorsViewModel;
        public ILocalizationService TranslationManager { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="IoCWindow"/> class.
        /// </summary>
        public IoCWindow()
            : base()
        {
            if (Debugger.IsAttached)
                Debug.Fail($"The constructor without parameters of {nameof(IoCWindow)} exists only for compatibility reasons.");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IoCWindow"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="vm">The vm.</param>
        public IoCWindow(ILocalizationService container, IUIColorsViewModel vm) : base()
        {
            TranslationManager = container;
            _colorsViewModel = vm;
            _colorsViewModel.PrimaryColorChanged += PrimaryColorChanged;
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

        private void PrimaryColorChanged(object sender, UiPrimaryColorEventArgs e)
        {
            var data = string.Empty;
            if (PackIcon.TryGet(PackIconKind.ApplicationIcon, out data))
            {
                var geo = Geometry.Parse(data);
                Icon = SetImage(geo, e.Color);
            }
        }

        /// <summary>
        /// Sets the image.
        /// </summary>
        /// <param name="geo">The geo.</param>
        /// <param name="color">The color.</param>
        /// <returns></returns>
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
