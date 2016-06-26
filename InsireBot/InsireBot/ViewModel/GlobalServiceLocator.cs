using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using InsireBotCore;
using InsireBot.Utils;
using System.Windows;
using System;

namespace InsireBot
{
    /// <summary>
    /// This class contains static references to all relevant ViewModels in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class GlobalServiceLocator
    {
        private static GlobalServiceLocator instance;
        public static GlobalServiceLocator Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GlobalServiceLocator();
                }
                return instance;
            }
        }

        private GlobalServiceLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<DrawerItemViewmodel>();

            if (ViewModelBase.IsInDesignModeStatic)
            {
                // Create design time view services and models
                SimpleIoc.Default.Register<IDataService, DesignTimeDataService>();
            }
            else
            {
                // Create run time view services and models
                SimpleIoc.Default.Register<IDataService, RuntimeDataService>();
            }

            SimpleIoc.Default.Register<DataParsingService>();
            SimpleIoc.Default.Register<MediaItemsStore>();
            SimpleIoc.Default.Register<PlaylistsViewModel>();
            SimpleIoc.Default.Register<MediaPlayerViewModel>();
            SimpleIoc.Default.Register<CreateMediaItemViewModel>();
            SimpleIoc.Default.Register<CreatePlaylistViewModel>();

        }

        public MediaPlayerViewModel MediaPlayerViewModel
        {
            get { return ServiceLocator.Current.GetInstance<MediaPlayerViewModel>(); }
        }

        public PlaylistsViewModel PlaylistsViewModel
        {
            get { return ServiceLocator.Current.GetInstance<PlaylistsViewModel>(); }
        }

        public DataParsingService DataParsingService
        {
            get { return ServiceLocator.Current.GetInstance<DataParsingService>(); }
        }

        public CreateMediaItemViewModel CreateMediaItemViewModel
        {
            get { return ServiceLocator.Current.GetInstance<CreateMediaItemViewModel>(); }
        }

        public CreatePlaylistViewModel CreatePlaylistViewModel
        {
            get { return ServiceLocator.Current.GetInstance<CreatePlaylistViewModel>(); }
        }

        public DrawerItemViewmodel DrawerItemViewmodel
        {
            get { return ServiceLocator.Current.GetInstance<DrawerItemViewmodel>(); }
        }

        public static void Cleanup()
        {
            SimpleIoc.Default.Unregister<DataParsingService>();
            SimpleIoc.Default.Unregister<MediaItemsStore>();
            SimpleIoc.Default.Unregister<PlaylistsViewModel>();
            SimpleIoc.Default.Unregister<MediaPlayerViewModel>();
            SimpleIoc.Default.Unregister<CreateMediaItemViewModel>();
            SimpleIoc.Default.Unregister<CreatePlaylistViewModel>();

            SimpleIoc.Default.Unregister<DrawerItemViewmodel>();
        }

        public void InvokeActionOnUiThread(Action action)
        {
            var dispatcher = Application.Current.Dispatcher;

            if (dispatcher.CheckAccess())
                action();
            else
                dispatcher.Invoke(action);
        }
    }
}