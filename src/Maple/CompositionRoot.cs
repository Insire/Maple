using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using DryIoc;
using FluentValidation;
using Maple.Domain;
using Maple.Youtube;
using Microsoft.Extensions.Logging;
using MvvmScarletToolkit;
using MvvmScarletToolkit.Abstractions;
using MvvmScarletToolkit.Observables;
using MvvmScarletToolkit.Wpf.FileSystemBrowser;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Extensions.Logging;
using Serilog.Formatting.Display;

namespace Maple
{
    public static class CompositionRoot
    {
        public static IContainer Get()
        {
            var c = new Container();

            RegisterLogging();
            RegisterViewModels();
            RegisterServices();
            RegisterValidation();
            RegisterControls();

            void RegisterLogging()
            {
                Directory.CreateDirectory("logs");
                var fileConfiguration = new LoggingFileConfiguration()
                {
                    PathFormat = "..\\logs\\{Date}.log",
                    FileSizeLimitBytes = 1073741824,
                    RetainedFileCountLimit = 1,
                };

                Log.Logger = CreateLogger(fileConfiguration);

                var loggerFactory = new LoggerFactory();

                loggerFactory
                    .AddSerilog();

                c.Register(serviceType: typeof(ILoggerProvider), implementationType: typeof(SerilogLoggerProvider), reuse: Reuse.Singleton);
                c.Register(serviceType: typeof(ILogger<>), implementationType: typeof(Logger<>));

                c.UseInstance<ILoggerFactory>(loggerFactory);
                c.UseInstance(Log.Logger);
            }

            Logger CreateLogger(LoggingFileConfiguration config)
            {
                var formatter = new MessageTemplateTextFormatter("{Timestamp:o} {RequestId,13} [{Level:u3}] ({ThreadId}) ({SourceContext}) {Message} ({EventId:x8}){NewLine}{Exception}", null);
                var logConfiguration = new LoggerConfiguration()
                    .MinimumLevel.Is(LogEventLevel.Verbose)
                    .Enrich.FromLogContext()
                    .Enrich.With<EventIdEnricher>()
                    .Enrich.With<ThreadIdEnricher>()
                    .Enrich.WithExceptionDetails()
                    .WriteTo.Console()
                    .WriteTo.Async(p => p.RollingFile(
                        formatter,
                        Environment.ExpandEnvironmentVariables(config.PathFormat),
                        fileSizeLimitBytes: config.FileSizeLimitBytes ?? LoggingFileConfiguration.DefaultFileSizeLimitBytes,
                        retainedFileCountLimit: config.RetainedFileCountLimit ?? LoggingFileConfiguration.DefaultRetainedFileCountLimit,
                        shared: true,
                        flushToDiskInterval: TimeSpan.FromSeconds(3)));

                return logConfiguration.CreateLogger();
            }

            void RegisterControls()
            {
                c.Register<Shell>();
                c.Register<SplashScreen>();
            }

            void RegisterViewModels()
            {
                c.RegisterMany(new[] { typeof(Playlists) }, typeof(Playlists), Reuse.Singleton);
                c.RegisterMany(new[] { typeof(MediaPlayers) }, typeof(MediaPlayers), Reuse.Singleton, setup: Setup.With(allowDisposableTransient: true));
                c.RegisterMany(new[] { typeof(Cultures) }, typeof(Cultures), Reuse.Singleton);
                c.RegisterMany(new[] { typeof(ILocalizationService), typeof(LocalizationsViewModel) }, typeof(LocalizationsViewModel), Reuse.Singleton);

                c.Register<AudioDevices>(setup: Setup.With(allowDisposableTransient: true));
                c.Register<FileSystemOptionsViewModel>(Reuse.Singleton);

                c.Register<NavigationViewModel>(Reuse.Singleton, setup: Setup.With(allowDisposableTransient: true));
                c.Register<ShellViewModel>(Reuse.Singleton);
                c.Register<PlaybackViewModel>(Reuse.Singleton);
                c.Register<DashboardViewModel>(Reuse.Singleton);
                c.Register<AboutViewModel>(Reuse.Singleton);
                c.Register<DialogViewModel>(Reuse.Singleton);
                c.Register<MetaDataViewModel>(Reuse.Singleton, setup: Setup.With(allowDisposableTransient: true));
                c.Register<FileSystemViewModel>(Reuse.Singleton, setup: Setup.With(allowDisposableTransient: true));
                c.Register<OptionsViewModel>(Reuse.Singleton, setup: Setup.With(allowDisposableTransient: true));

                c.Register<YoutubeImportViewModel>(setup: Setup.With(allowDisposableTransient: true));

                c.Register<SplashScreenViewModel>(Reuse.Singleton);

                c.UseInstance(new ReadOnlyObservableCollection<RepeatMode>(new ObservableCollection<RepeatMode>(Enum.GetValues(typeof(RepeatMode)).Cast<RepeatMode>())));
            };

            void RegisterServices()
            {
                c.Register<MediaPlayerFactory, MediaPlayerFactory>(Reuse.Singleton);
                c.Register<PlaylistFactory, PlaylistFactory>(Reuse.Singleton);
                c.Register<ISequenceService, SequenceService>();
                c.Register<IYoutubeService, YoutubeService>(Reuse.Singleton);
                c.Register<IAudioDeviceProvider, AudioDeviceProvider>(Reuse.Singleton);

                c.Register<IVersionService, VersionService>(Reuse.Singleton);
                c.Register<ILocalizationProvider, ResxTranslationProvider>(Reuse.Singleton);

                c.RegisterMany(new[] { typeof(IScarletCommandBuilder), typeof(IMapleCommandBuilder) }, typeof(MapleCommandBuilder), Reuse.Singleton);
                c.Register<IBusyStack, BusyStack>();

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

            return c;
        }
    }
}
