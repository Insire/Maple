using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using AdonisUI.Controls;
using Maple.Domain;
using MvvmScarletToolkit;
using MvvmScarletToolkit.Abstractions;

namespace Maple
{
    public class IoCWindow : AdonisWindow, IIocFrameworkElement
    {
        private readonly IScarletMessenger _messenger;

        public ILocalizationService LocalizationService { get; }
        public IScarletEventManager<INotifyPropertyChanged, PropertyChangedEventArgs> WeakEventManager { get; }
        public IScarletCommandBuilder CommandBuilder { get; }

        public IoCWindow()
            : base()
        {
            if (Debugger.IsAttached)
                Debug.Fail($"The constructor without parameters of {nameof(IoCWindow)} exists only for compatibility reasons.");
        }

        protected IoCWindow(IScarletCommandBuilder commandBuilder, ILocalizationService localizationService)
            : base()
        {
            LocalizationService = localizationService ?? throw new ArgumentNullException(nameof(localizationService));
            CommandBuilder = commandBuilder ?? throw new ArgumentNullException(nameof(commandBuilder));
            WeakEventManager = CommandBuilder.WeakEventManager ?? throw new ArgumentNullException(nameof(IScarletCommandBuilder.WeakEventManager));

            _messenger = CommandBuilder.Messenger ?? throw new ArgumentNullException(nameof(IScarletCommandBuilder.Messenger));
            _messenger.Subscribe<UiPrimaryColorChangedMessage>(PrimaryColorChanged);

            UpdateImage(Colors.DarkOrange);
        }

        public Task Invoke(Action action, CancellationToken token)
        {
            return CommandBuilder.Dispatcher.Invoke(action, token);
        }

        private void PrimaryColorChanged(UiPrimaryColorChangedMessage e)
        {
            UpdateImage(e.Content);
        }

        private void UpdateImage(Color color)
        {
            if (MaplePackIcon.TryGet(PackIconKind.ApplicationIcon, out var data))
            {
                var geo = Geometry.Parse(data);
                var image = GetImage(geo, color);
                SetCurrentValue(IconProperty, image);
            }
        }

        private BitmapSource GetImage(Geometry geo, Color color)
        {
            var canvas = new Canvas
            {
                Width = 36,
                Height = 36,
                Background = new SolidColorBrush(Colors.Transparent)
            };

            canvas.Children.Add(new System.Windows.Shapes.Path()
            {
                Data = geo,
                Stretch = Stretch.Fill,
                Fill = new SolidColorBrush(color),
                Width = 36,
                Height = 36,
            });

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
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.StreamSource = memory;
                bitmapImage.UriSource = null;
                bitmapImage.EndInit();

                bitmapImage.Freeze();

                return bitmapImage;
            }
        }
    }
}
