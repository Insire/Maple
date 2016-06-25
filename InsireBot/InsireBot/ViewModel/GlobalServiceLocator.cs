using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using InsireBotCore;
using InsireBot.Utils;

namespace InsireBot.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class GlobalServiceLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public GlobalServiceLocator()
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

            SimpleIoc.Default.Unregister<DrawerItemViewmodel>();
        }
    }
}