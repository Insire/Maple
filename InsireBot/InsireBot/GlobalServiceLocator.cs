using System;

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

        /// <summary>
        /// hopefully obsolete, if i can figure out how to provide an instance of TranslationManager to nested usercontrols without code behind
        /// </summary>
        public ITranslationManager TranslationManager
        {
            get { return _shellViewModel.TranslationManager; }
        }
    }
}