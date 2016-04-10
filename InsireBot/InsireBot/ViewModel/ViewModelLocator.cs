using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using InsireBotCore;

namespace InsireBot.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

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

            SimpleIoc.Default.Register<NewMediaItemsDialogViewModel>();
            SimpleIoc.Default.Register<NewMediaItemViewModel>();
            SimpleIoc.Default.Register<NewPlaylistViewModel>();
            SimpleIoc.Default.Register<MediaPlayerViewModel>();
        }

        public MediaPlayerViewModel MediaPlayerViewModel
        {
            get { return ServiceLocator.Current.GetInstance<MediaPlayerViewModel>(); }
        }

        public NewMediaItemsDialogViewModel NewMediaItemsDialogViewModel
        {
            get { return ServiceLocator.Current.GetInstance<NewMediaItemsDialogViewModel>(); }
        }

        public NewPlaylistViewModel NewPlaylistViewModel
        {
            get { return ServiceLocator.Current.GetInstance<NewPlaylistViewModel>(); }
        }

        public NewMediaItemViewModel NewMediaItemViewModel
        {
            get { return ServiceLocator.Current.GetInstance<NewMediaItemViewModel>(); }
        }

        public static void Cleanup()
        {
            SimpleIoc.Default.Unregister<NewMediaItemsDialogViewModel>();
            SimpleIoc.Default.Unregister<NewMediaItemViewModel>();
            SimpleIoc.Default.Unregister<NewPlaylistViewModel>();
            SimpleIoc.Default.Unregister<MediaPlayerViewModel>();
        }
    }
}