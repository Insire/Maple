using Maple.Localization.Properties;
using System;
using System.Threading.Tasks;

namespace Maple.Core
{
    /// <summary>
    /// Decorator logs loading and saving method calls
    /// </summary>
    public class RefreshableDecorator : IRefreshable
    {
        private readonly IRefreshable _refreshable;
        private readonly IMapleLog _log;

        public RefreshableDecorator(IRefreshable refreshable, IMapleLog log)
        {
            _refreshable = refreshable ?? throw new ArgumentNullException(nameof(refreshable));
            _log = log ?? throw new ArgumentNullException(nameof(log));
        }

        public Task LoadAsync()
        {
            _log.Info($"{Resources.Loading} {_refreshable.GetType().Name}");
            return _refreshable.LoadAsync();
        }

        public Task SaveAsync()
        {
            _log.Info($"{Resources.Saving} {_refreshable.GetType().Name}");
            return _refreshable.SaveAsync();
        }
    }
}
