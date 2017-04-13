using Maple.Core;
using System;
using System.ComponentModel;
using System.Windows;

namespace Maple
{
    public class LanguageChangedEventManager : WeakEventManager
    {
        public static void AddListener(ITranslationService source, IWeakEventListener listener)
        {
            CurrentManager.ProtectedAddListener(source, listener);
        }

        /// <summary>
        /// Removes the listener.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="listener">The listener.</param>
        public static void RemoveListener(ITranslationService source, IWeakEventListener listener)
        {
            CurrentManager.ProtectedRemoveListener(source, listener);
        }

        private void OnLanguageChanged(object sender, EventArgs e)
        {
            DeliverEvent(sender, e);
        }

        protected override void StartListening(object source)
        {
            var manager = source as ITranslationService;
            if (manager == null)
                return;

            manager.PropertyChanged += PropertyChanged;
        }

        private void PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ITranslationService.CurrentLanguage))
                DeliverEvent(sender, new EventArgs());
        }

        protected override void StopListening(object source)
        {
            var manager = source as ITranslationService;
            if (manager == null)
                return;

            manager.PropertyChanged -= PropertyChanged;
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
