using DryIoc;
using FluentValidation;
using Jot;
using Jot.Storage;
using Maple.Domain;
using Maple.Youtube;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MvvmScarletToolkit;
using MvvmScarletToolkit.Abstractions;
using MvvmScarletToolkit.Observables;
using MvvmScarletToolkit.Wpf.Features.FileSystemBrowser;
using MvvmScarletToolkit.Wpf.FileSystemBrowser;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Extensions.Logging;
using Serilog.Formatting.Display;
using SoftThorn.MonstercatNet;
using System;
using System.ComponentModel;
using System.IO;
using System.Net.Http;
using System.Windows;

namespace Maple
{
    public static class CompositionRoot
    {
        public static DryIoc.IContainer Get()
        {
            var assemblyName = typeof(CompositionRoot).Assembly.GetName();
            var version = assemblyName.Version;

            var settingsDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "SoftThorn", "Maple");
            var logDirectory = Path.Combine(settingsDirectory, "logs");

            Directory.CreateDirectory(settingsDirectory);
            Directory.CreateDirectory(logDirectory);

            var c = new DryIoc.Container();

            RegisterLogging();
            RegisterViewModels();
            RegisterServices();
            RegisterValidation();
            RegisterGui();
            RegisterDataAccess();
            RegisterFileSystemAccess();
            RegisterMonstercat();
            RegisterStartup();

            void RegisterLogging()
            {
                var formatter = new MessageTemplateTextFormatter("{Timestamp:o} {RequestId,13} [{Level:u3}] ({SourceContext}) ({ThreadId}) ({EventId:x8}) {Message}{NewLine}{Exception}", null);
                var logConfiguration = new LoggerConfiguration()
                    .MinimumLevel.Is(LogEventLevel.Verbose)
                    .Enrich.FromLogContext()
                    .Enrich.With<EventIdEnricher>()
                    .Enrich.With<ThreadIdEnricher>()
                    .Enrich.WithExceptionDetails()
                    .WriteTo.Console()
                    .WriteTo.Async(p => p.RollingFile(
                        formatter,
                        Environment.ExpandEnvironmentVariables(Path.Combine(logDirectory, $"{version}_maple.log")),
                        fileSizeLimitBytes: 1073741824,
                        retainedFileCountLimit: 1,
                        shared: true,
                        flushToDiskInterval: TimeSpan.FromSeconds(3)));

                Log.Logger = logConfiguration.CreateLogger();

                var loggerFactory = new LoggerFactory();

                loggerFactory
                    .AddSerilog();

                c.Register(serviceType: typeof(ILoggerProvider), implementationType: typeof(SerilogLoggerProvider), reuse: Reuse.Singleton);
                c.Register(serviceType: typeof(ILogger<>), implementationType: typeof(Logger<>));

                c.UseInstance<ILoggerFactory>(loggerFactory);
                c.UseInstance(Log.Logger);
            }

            void RegisterGui()
            {
                var tracker = new Tracker(new JsonFileStore(perUser: true));
                tracker.Configure<Shell>()
                    .Id(w => $"[Width={SystemParameters.VirtualScreenWidth},Height{SystemParameters.VirtualScreenHeight}]")
                    .Properties(w => new { w.Height, w.Width, w.Left, w.Top, w.WindowState })
                    .PersistOn(nameof(Window.Closing))
                    .StopTrackingOn(nameof(Window.Closing));

                c.UseInstance(new Uri("/Maple;component/Resources/Style.xaml", UriKind.RelativeOrAbsolute));
                c.UseInstance(tracker);

                c.Register<Shell>(Reuse.Singleton);
                c.Register<SplashScreen>();
                c.Register<IoCResourceDictionary>(Reuse.Singleton, Made.Of(() => new IoCResourceDictionary(Arg.Of<ILocalizationService>(), Arg.Of<IScarletEventManager<INotifyPropertyChanged, PropertyChangedEventArgs>>(), Arg.Of<Uri>())));
            }

            void RegisterViewModels()
            {
                c.RegisterMany(new[] { typeof(Playlists) }, typeof(Playlists), Reuse.Singleton);
                c.RegisterMany(new[] { typeof(MediaPlayers) }, typeof(MediaPlayers), Reuse.Singleton, setup: Setup.With(allowDisposableTransient: true));
                c.RegisterMany(new[] { typeof(Cultures) }, typeof(Cultures), Reuse.Singleton);
                c.RegisterMany(new[] { typeof(ILocalizationService), typeof(LocalizationsViewModel) }, typeof(LocalizationsViewModel), Reuse.Singleton);

                c.Register<AudioDevices>(setup: Setup.With(allowDisposableTransient: true));

                c.Register<PlaybackViewModel>(Reuse.Singleton);

                c.Register<OptionsViewModel>(Reuse.Singleton, setup: Setup.With(allowDisposableTransient: true));

                c.Register<YoutubeImportViewModel>(setup: Setup.With(allowDisposableTransient: true));
            };

            void RegisterServices()
            {
                c.Register<MediaPlayerFactory, MediaPlayerFactory>(Reuse.Singleton);
                c.Register<PlaylistFactory, PlaylistFactory>(Reuse.Singleton);
                c.Register<AudioDeviceFactory, AudioDeviceFactory>(Reuse.Transient);
                c.Register<ISequenceService, SequenceService>();
                c.Register<IYoutubeService, YoutubeService>(Reuse.Singleton);
                c.Register<IAudioDeviceProvider, AudioDeviceProvider>(Reuse.Singleton);

                c.Register<IVersionService, VersionService>(Reuse.Singleton);
                c.Register<ILocalizationProvider, ResxTranslationProvider>(Reuse.Singleton);

                c.Register<IBusyStack, BusyStack>();

                c.UseInstance(ScarletCommandBuilder.Default);
                c.UseInstance(ScarletDispatcher.Default);
                c.UseInstance(ScarletMessenger.Default);
                c.UseInstance(ScarletCommandManager.Default);
                c.UseInstance(ScarletMessageProxy.Default);
                c.UseInstance(ScarletWeakEventManager.Default);
                c.UseInstance(ScarletExitService.Default);
            }

            void RegisterValidation()
            {
                c.Register<IValidator<Playlist>, PlaylistValidator>(Reuse.Singleton);
                c.Register<IValidator<Playlists>, PlaylistsValidator>(Reuse.Singleton);
                c.Register<IValidator<MediaPlayer>, MediaPlayerValidator>(Reuse.Singleton);
                c.Register<IValidator<MediaPlayers>, MediaPlayersValidator>(Reuse.Singleton);
                c.Register<IValidator<MediaItem>, MediaItemValidator>(Reuse.Singleton);
            }

            void RegisterDataAccess()
            {
                var path = Path.Combine(settingsDirectory, "maple.db");
                var builder = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                    .UseSqlite($"Data Source={path};");

                c.UseInstance(builder.Options);
                c.Register<ApplicationDbContext, ApplicationDbContext>(Reuse.Transient, setup: Setup.With(allowDisposableTransient: true));
            }

            void RegisterFileSystemAccess()
            {
                c.UseInstance(FileSystemOptionsViewModel.Default);
                c.Register<FileSystemViewModel>(Reuse.Transient, setup: Setup.With(allowDisposableTransient: true));
                c.Register<IFileSystemViewModelFactory, FileSystemViewModelFactory>();
            }

            void RegisterMonstercat()
            {
                c.Register<IMonstercatApi>(Reuse.Singleton, Made.Of(() => MonstercatApi.Create(Arg.Of<HttpClient>())));
                c.Register<MonstercatImportViewModel>(Reuse.Singleton);
                c.Register<HttpClient>(Reuse.Singleton, Made.Of(() => new HttpClient()));
            }

            void RegisterStartup()
            {
                c.Register<SplashScreenViewModel>(Reuse.Singleton);
                c.Register<ShellViewModel>(Reuse.Singleton);
                c.Register<NavigationViewModel>(Reuse.Singleton, setup: Setup.With(allowDisposableTransient: true));
                c.Register<DashboardViewModel>(Reuse.Singleton);
                c.Register<AboutViewModel>(Reuse.Singleton);
                c.Register<MetaDataViewModel>(Reuse.Singleton, setup: Setup.With(allowDisposableTransient: true));
            }

            return c;
        }
    }
}
