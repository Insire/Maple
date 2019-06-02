using System;
using System.Diagnostics;
using MvvmScarletToolkit.Abstractions;

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
        public ILocalizationService LocalizationService { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="IoCResourceDictionary"/> class.
        /// </summary>
        public IoCResourceDictionary()
            : base()
        {
            if (Debugger.IsAttached)
                Debug.Fail($"The constructor without parameters of {nameof(IoCResourceDictionary)} exists only for compatibility reasons.");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IoCResourceDictionary"/> class.
        /// </summary>
        /// <param name="translationManager">The translation manager.</param>
        public IoCResourceDictionary(ILocalizationService localizationService, Uri source)
            : base()
        {
            LocalizationService = localizationService ?? throw new ArgumentNullException(nameof(localizationService));
            Source = source ?? throw new ArgumentNullException(nameof(source));

            Add(typeof(ILocalizationService).Name, localizationService);
        }
    }
}
