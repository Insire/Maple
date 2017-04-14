using Maple.Localization.Properties;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Maple.Core
{
    /// <summary>
    /// Decorator logs loading and saving method calls
    /// </summary>
    /// <seealso cref="Maple.Core.ILoadableViewModel" />
    public class RefreshableDecorator : ILoadableViewModel
    {
        private readonly ILoadableViewModel _refreshable;
        private readonly IMapleLog _log;

        /// <summary>
        /// Gets a value indicating whether this instance is loaded.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is loaded; otherwise, <c>false</c>.
        /// </value>
        public bool IsLoaded => _refreshable.IsLoaded;

        /// <summary>
        /// Gets the load command.
        /// </summary>
        /// <value>
        /// The load command.
        /// </value>
        public ICommand LoadCommand => _refreshable.LoadCommand;
        /// <summary>
        /// Gets the refresh command.
        /// </summary>
        /// <value>
        /// The refresh command.
        /// </value>
        public ICommand RefreshCommand => _refreshable.RefreshCommand;

        /// <summary>
        /// Initializes a new instance of the <see cref="RefreshableDecorator"/> class.
        /// </summary>
        /// <param name="refreshable">The refreshable.</param>
        /// <param name="log">The log.</param>
        /// <exception cref="System.ArgumentNullException">
        /// refreshable
        /// or
        /// log
        /// </exception>
        public RefreshableDecorator(ILoadableViewModel refreshable, IMapleLog log)
        {
            _refreshable = refreshable ?? throw new ArgumentNullException(nameof(refreshable));
            _log = log ?? throw new ArgumentNullException(nameof(log));
        }

        /// <summary>
        /// Loads this instance.
        /// </summary>
        public void Load()
        {
            _log.Info($"{Resources.Loading} {_refreshable.GetType().Name}");
            _refreshable.Load();
        }

        /// <summary>
        /// Saves this instance.
        /// </summary>
        public void Save()
        {
            _log.Info($"{Resources.Saving} {_refreshable.GetType().Name}");
            _refreshable.Save();
        }

        public async Task SaveAsync()
        {
            _log.Info($"{Resources.Saving} {_refreshable.GetType().Name}");
            await _refreshable.LoadAsync();
        }

        public async Task LoadAsync()
        {
            _log.Info($"{Resources.Loading} {_refreshable.GetType().Name}");
            await _refreshable.LoadAsync();
        }
    }
}
