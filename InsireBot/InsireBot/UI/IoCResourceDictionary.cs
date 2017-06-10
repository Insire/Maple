using Maple.Core;
using System;
using System.Diagnostics;

namespace Maple
{
    /// <summary>
    /// SharedResourceDictionary to support <see cref="TranslationExtension" />
    /// </summary>
    /// <seealso cref="Maple.SharedResourceDictionary" />
    /// <seealso cref="Maple.Core.IIocFrameworkElement" />
    public class IoCResourceDictionary : SharedResourceDictionary, IIocFrameworkElement
    {
        /// <summary>
        /// Gets the translation manager.
        /// </summary>
        /// <value>
        /// The translation manager.
        /// </value>
        public ILocalizationService TranslationManager { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="IoCResourceDictionary"/> class.
        /// </summary>
        public IoCResourceDictionary() : base()
        {
            Debug.Fail($"The constructor without parameters of {nameof(IoCResourceDictionary)} exists only for compatibility reasons.");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IoCResourceDictionary"/> class.
        /// </summary>
        /// <param name="translationManager">The translation manager.</param>
        public IoCResourceDictionary(ILocalizationService service, Uri url) : base()
        {
            TranslationManager = service;
            Source = url;
            Add(typeof(ILocalizationService).Name, service);
        }
    }
}
