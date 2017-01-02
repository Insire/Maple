using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using InsireBotCore;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Windows;

namespace InsireBotWPF
{
    /// <summary>
    /// This class contains static references to all relevant ViewModels in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class GlobalServiceLocator
    {
        private static GlobalServiceLocator _instance;
        public static GlobalServiceLocator Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new GlobalServiceLocator();

                return _instance;
            }
        }

        private GlobalServiceLocator()
        {
            App.Log.Info("Loading Services");

            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<Scenes>();
            SimpleIoc.Default.Register<UIColorsViewModel>();

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

            App.Log.Info("Loaded Services");
        }

        public IDataService DataService
        {
            get { return ServiceLocator.Current.GetInstance<IDataService>(); }
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

        public Scenes Scenes
        {
            get { return ServiceLocator.Current.GetInstance<Scenes>(); }
        }

        public UIColorsViewModel UIColorsViewModel
        {
            get { return ServiceLocator.Current.GetInstance<UIColorsViewModel>(); }
        }


        public static void Cleanup()
        {
            App.Log.Info("Unloading Services");
            SimpleIoc.Default.Unregister<DataParsingService>();
            SimpleIoc.Default.Unregister<MediaItemsStore>();
            SimpleIoc.Default.Unregister<PlaylistsViewModel>();
            SimpleIoc.Default.Unregister<MediaPlayerViewModel>();
            SimpleIoc.Default.Unregister<CreateMediaItemViewModel>();
            SimpleIoc.Default.Unregister<CreatePlaylistViewModel>();

            SimpleIoc.Default.Unregister<UIColorsViewModel>();
            SimpleIoc.Default.Unregister<Scenes>();
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