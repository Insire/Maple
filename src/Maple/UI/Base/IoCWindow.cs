using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using MahApps.Metro.Controls;
using Maple.Core;
using Maple.Domain;
using Maple.Icons;
using MvvmScarletToolkit.Abstractions;

namespace Maple
{
    public abstract class IoCWindow : MetroWindow, IIocFrameworkElement
    {
        private readonly IScarletMessenger _messenger;

        public ILocalizationService LocalizationService { get; }

        protected IoCWindow()
            : base()
        {
            if (Debugger.IsAttached)
                Debug.Fail($"The constructor without parameters of {nameof(IoCWindow)} exists only for compatibility reasons.");
        }

        protected IoCWindow(ILocalizationService localizationService, IScarletMessenger messenger)
            : base()
        {
            LocalizationService = localizationService ?? throw new ArgumentNullException(nameof(localizationService));
            _messenger = messenger ?? throw new ArgumentNullException(nameof(messenger));

            _messenger.Subscribe<UiPrimaryColorChangedMessage>(PrimaryColorChanged);
        }

        private void PrimaryColorChanged(UiPrimaryColorChangedMessage e)
        {
            if (MaplePackIcon.TryGet(PackIconKind.ApplicationIcon, out var data))
            {
                var geo = Geometry.Parse(data);
                SetCurrentValue(IconProperty, SetImage(geo, e.Content));
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
