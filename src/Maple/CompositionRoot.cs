using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DryIoc;
using FluentValidation;
using Maple.Data;
using Maple.Domain;
using Maple.Youtube;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MvvmScarletToolkit;
using MvvmScarletToolkit.Abstractions;
using MvvmScarletToolkit.FileSystemBrowser;
using MvvmScarletToolkit.Observables;
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
        private const string DatabasePath = "..\\maple.db";

        private const string InMemorySqlite = "InMemorySqlite";
        private const string DebugFileSqlite = "DebugFileSqlite";
        private const string Debug = "Debug";

        private static (bool MigrationRequired, bool SeedRequired, bool CreationRequired, bool InMemory) HandleStart(string[] args)
        {
            var seedRequired = false;
            var migrationRequired = false;
            var creationRequired = false;
            var inMemory = false;

            if (args.Any(p => p.IndexOf(InMemorySqlite, StringComparison.InvariantCultureIgnoreCase) >= 0))
            {
                // not working
                seedRequired = true;
                migrationRequired = true;
                inMemory = true;

                return (migrationRequired, seedRequired, creationRequired, inMemory);
            }

            if (args.Any(p => p.IndexOf(DebugFileSqlite, StringComparison.InvariantCultureIgnoreCase) >= 0))
            {
                // not working
                seedRequired = true;
                migrationRequired = true;

                return (migrationRequired, seedRequired, creationRequired, inMemory);
            }

            if (args.Any(p => p.IndexOf(Debug, StringComparison.InvariantCultureIgnoreCase) >= 0))
            {
                if (!File.Exists(DatabasePath))
                {
                    creationRequired = true;
                    seedRequired = true;
                }

                migrationRequired = true;

                return (migrationRequired, seedRequired, creationRequired, inMemory);
            }

            if (!File.Exists(DatabasePath))
            {
                // working?
                creationRequired = true;
            }

            migrationRequired = true;

            return (migrationRequired, seedRequired, creationRequired, inMemory);
        }

        public static async Task<IContainer> Get(string[] args)
        {
            var (MigrationRequired, SeedRequired, CreationRequired, InMemory) = HandleStart(args);

            var builder = new SqliteConnectionStringBuilder($"Data Source={DatabasePath};")
            {
                Mode = InMemory ? SqliteOpenMode.Memory : CreationRequired ? SqliteOpenMode.ReadWriteCreate : SqliteOpenMode.ReadWrite,
            };

            var container = CreateContainer(builder.ToString());

            var context = container.Resolve<IUnitOfWork>();
            if (MigrationRequired)
            {
                context.Migrate().Wait();
            }

            if (SeedRequired)
            {
                var factory = new DesignTimeDataFactory(context);
                factory.SeedDatabase().Wait();
            }

            return container;
        }

        private static IContainer CreateContainer(string connectionString)
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

                c.Register<Splat.ILogger, SquirrelLogger>(setup: Setup.DecoratorWith(order: 2));
                c.Register(serviceType: typeof(ILoggerProvider), implementationType: typeof(SerilogLoggerProvider), reuse: Reuse.Singleton);
                c.Register(serviceType: typeof(ILogger<>), implementationType: typeof(Logger<>));

                c.UseInstance<ILoggerFactory>(loggerFactory);
                c.UseInstance(Log.Logger);

                c.UseInstance(new DbContextOptionsBuilder<PlaylistContext>()
                    .UseSqlite(connectionString) // TODO switch for inmemory
                    .UseLoggerFactory(loggerFactory)
                    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                    .Options);
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
                c.Register<DialogViewModel>(Reuse.Singleton);
                c.Register<MetaDataViewModel>(Reuse.Singleton, setup: Setup.With(allowDisposableTransient: true));
                c.Register<FileSystemViewModel>(Reuse.Singleton, setup: Setup.With(allowDisposableTransient: true));
                c.Register<OptionsViewModel>(Reuse.Singleton, setup: Setup.With(allowDisposableTransient: true));

                c.Register<YoutubeImportViewModel>(setup: Setup.With(allowDisposableTransient: true));

                c.Register<SplashScreenViewModel>(Reuse.Singleton);
            };

            void RegisterServices()
            {
                c.Register<PlaylistContext>(Reuse.Transient, setup: Setup.With(allowDisposableTransient: true));

                c.Register<IUnitOfWork, MapleUnitOfWork>(Reuse.Transient, setup: Setup.With(allowDisposableTransient: true));

                c.Register<ISequenceService, SequenceService>();
                c.Register<IYoutubeService, YoutubeService>();

                c.Register<IVersionService, VersionService>(Reuse.Singleton);
                c.Register<ILocalizationProvider, ResxTranslationProvider>(Reuse.Singleton);

                c.UseInstance(ScarletDispatcher.Default);
                c.RegisterMany(new[] { typeof(IScarletCommandBuilder), typeof(IMapleCommandBuilder) }, typeof(MapleCommandBuilder), Reuse.Singleton);
                c.Register<IBusyStack, BusyStack>();
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
