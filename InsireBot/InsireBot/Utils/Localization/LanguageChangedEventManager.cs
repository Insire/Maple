using System;
using System.Windows;

namespace InsireBot
{
    public class LanguageChangedEventManager : WeakEventManager
    {
        public static void AddListener(ITranslationManager source, IWeakEventListener listener)
        {
            CurrentManager.ProtectedAddListener(source, listener);
        }

        public static void RemoveListener(ITranslationManager source, IWeakEventListener listener)
        {
            CurrentManager.ProtectedRemoveListener(source, listener);
        }

        private void OnLanguageChanged(object sender, EventArgs e)
        {
            DeliverEvent(sender, e);
        }

        protected override void StartListening(object source)
        {
            var manager = (ITranslationManager)source;
            manager.LanguageChanged += OnLanguageChanged;
        }

        protected override void StopListening(object source)
        {
            var manager = (ITranslationManager)source;
            manager.LanguageChanged -= OnLanguageChanged;
        }

        private static LanguageChangedEventManager CurrentManager
        {
            get
            {
                var managerType = typeof(LanguageChangedEventManager);
                var manager = GetCurrentManager(managerType) as LanguageChangedEventManager;

                if (manager == null)
                {
                    manager = new LanguageChangedEventManager();
                    SetCurrentManager(managerType, manager);
                }

                return manager;
            }
        }
    }
}
