using System;
using System.Windows;

namespace InsireBot
{
    public class GlobalServiceLocator
    {
        private ShellViewModel _shellViewModel;

        private static GlobalServiceLocator _instance;
        public static GlobalServiceLocator Instance
        {
            get { return _instance; }
        }

        public GlobalServiceLocator(ShellViewModel shellViewModel)
        {
            if (_instance == null)
            {
                _instance = this;
                _shellViewModel = shellViewModel;
            }
            else
                throw new InvalidOperationException($"can't initalize {nameof(GlobalServiceLocator)} more than once");
        }

        public ITranslationManager TranslationManager
        {
            get { return _shellViewModel.TranslationManager; }
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