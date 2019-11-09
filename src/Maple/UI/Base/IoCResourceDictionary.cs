using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using MvvmScarletToolkit.Abstractions;

namespace Maple
{
    /// <summary>
    /// <para>This ResourceDictionary is a specialized ResourceDictionary, that loads it content only once. </para>
    /// <para>If a second instance with the same source is created, it merges the resources from the cache, without reloading it from disk.</para>
    /// <para>It also adds support for <see cref="TranslationExtension" /></para>
    /// <para>Original idea from https://www.wpftutorial.net/MergedDictionaryPerformance.html</para>
    /// <para>see also:
    ///     <list type="bullet">
    ///         <item><seealso cref="System.Windows.ResourceDictionary" /></item>
    ///         <item><seealso cref="Maple.Core.IIocFrameworkElement" /></item>
    ///     </list>
    /// </para>
    /// </summary>
    public class IoCResourceDictionary : ResourceDictionary, IIocFrameworkElement
    {
        public static readonly Dictionary<Uri, ResourceDictionary> _sharedDictionaries = new Dictionary<Uri, ResourceDictionary>();

        public ILocalizationService LocalizationService { get; }

        public IScarletEventManager<INotifyPropertyChanged, PropertyChangedEventArgs> WeakEventManager { get; }

        public IoCResourceDictionary()
            : base()
        {
            if (Debugger.IsAttached)
                Debug.Fail($"The constructor without parameters of {nameof(IoCResourceDictionary)} exists only for compatibility reasons.");
        }

        public IoCResourceDictionary(ILocalizationService localizationService, IScarletEventManager<INotifyPropertyChanged, PropertyChangedEventArgs> weakEventManager, Uri source)
            : base()
        {
            LocalizationService = localizationService ?? throw new ArgumentNullException(nameof(localizationService));
            WeakEventManager = weakEventManager ?? throw new ArgumentNullException(nameof(weakEventManager));
            Source = source ?? throw new ArgumentNullException(nameof(source));

            Add(typeof(IScarletEventManager<INotifyPropertyChanged, PropertyChangedEventArgs>).Name, weakEventManager);
            Add(typeof(ILocalizationService).Name, localizationService);
        }

        private Uri _sourceUri;
        /// <summary>
        /// Gets or sets the uniform resource identifier (URI) to load resources from.
        /// </summary>
        public new Uri Source
        {
            get { return _sourceUri; }
            set
            {
                _sourceUri = value;

                if (!_sharedDictionaries.ContainsKey(value))
                {
                    // If the dictionary is not yet loaded, load it by setting
                    // the source of the base class
                    base.Source = value;

                    // add it to the cache
                    _sharedDictionaries.Add(value, this);
                }
                else
                {
                    // If the dictionary is already loaded, get it from the cache
                    MergedDictionaries.Add(_sharedDictionaries[value]);
                }
            }
        }
    }
}
