using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Bogus;
using DryIoc;
using FluentValidation;
using Maple.Data;
using Maple.Domain;
using Maple.Youtube;
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
        public static async Task<IContainer> Get(string[] args)
        {
            var c = new Container();

            await Task.Run(() => InitializeContainer());

            if (args.Contains("inmemory"))
            {
                var factory = new DesignTimeDataFactory(c.Resolve<IUnitOfWork>());
                await Task.Run(async () => await factory.SeedDatabase());
            }

            return c;

            IContainer InitializeContainer()
            {
                RegisterLogging();
                RegisterViewModels();
                RegisterServices();
                RegisterValidation();
                RegisterControls();

                return c;
            }

            void RegisterLogging()
            {
                Directory.CreateDirectory("logs");
                var fileConfiguration = new LoggingFileConfiguration()
                {
                    PathFormat = "logs\\{Date}.log",
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

                if (args.Contains("inmemory"))
                {
                    c.UseInstance(new DbContextOptionsBuilder<PlaylistContext>()
                        .UseInMemoryDatabase("Maple")
                        .UseLoggerFactory(loggerFactory)
                        .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                        .Options);
                }
                else
                {
                    //c.UseInstance(new DbContextOptionsBuilder<PlaylistContext>()
                    //    .UseSqlite("Data Source=../maple.db;")
                    //    .UseLoggerFactory(loggerFactory)
                    //    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                    //    .Options);
                }
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
                // TODO register disposeables

                c.RegisterMany(new[] { typeof(Playlists) }, typeof(Playlists), Reuse.Singleton);
                c.RegisterMany(new[] { typeof(MediaPlayers) }, typeof(MediaPlayers), Reuse.Singleton, setup: Setup.With(allowDisposableTransient: true));
                c.RegisterMany(new[] { typeof(Cultures) }, typeof(Cultures), Reuse.Singleton);

                //generic ViewModels
                c.Register<NavigationViewModel>(Reuse.Singleton, setup: Setup.With(allowDisposableTransient: true));
                c.Register<AudioDevices>(Reuse.Singleton, setup: Setup.With(allowDisposableTransient: true));
                c.Register<ShellViewModel>(Reuse.Singleton);
                c.Register<DialogViewModel>(Reuse.Singleton);
                c.Register<MetaDataViewModel>(Reuse.Singleton, setup: Setup.With(allowDisposableTransient: true));
                c.Register<FileSystemOptionsViewModel>();
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
                c.Register<ILocalizationService, LocalizationsViewModel>(Reuse.Singleton);
                c.Register<ILocalizationProvider, ResxTranslationProvider>(Reuse.Singleton);

                c.UseInstance(ScarletDispatcher.Default);
                c.RegisterMany(new[] { typeof(ICommandBuilder), typeof(IMapleCommandBuilder) }, typeof(MapleCommandBuilder), Reuse.Singleton);
                c.Register<IBusyStack, BusyStack>();
                c.Register<IScarletMessenger, ScarletMessenger>(Reuse.Singleton);
                c.Register<IScarletCommandManager, ScarletCommandManager>(Reuse.Singleton);
                c.Register<IScarletMessageProxy, DefaultMessageProxy>(Reuse.Singleton);
            }

            void RegisterValidation()
            {
                c.Register<IValidator<Playlist>, PlaylistValidator>(Reuse.Singleton);
                c.Register<IValidator<Playlists>, PlaylistsValidator>(Reuse.Singleton);
                c.Register<IValidator<MediaPlayer>, MediaPlayerValidator>(Reuse.Singleton);
                c.Register<IValidator<MediaPlayers>, MediaPlayersValidator>(Reuse.Singleton);
                c.Register<IValidator<MediaItem>, MediaItemValidator>(Reuse.Singleton);
            }
        }
    }
}
