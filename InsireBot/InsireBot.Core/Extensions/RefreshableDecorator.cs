using Maple.Localization.Properties;
using System;
using System.Windows.Input;

namespace Maple.Core
{
    /// <summary>
    /// Decorator logs loading and saving method calls
    /// </summary>
    public class RefreshableDecorator : ILoadableViewModel
    {
        private readonly ILoadableViewModel _refreshable;
        private readonly IMapleLog _log;

        public bool IsLoaded => _refreshable.IsLoaded;

        public ICommand LoadCommand => _refreshable.LoadCommand;
        public ICommand RefreshCommand => _refreshable.RefreshCommand;

        public RefreshableDecorator(ILoadableViewModel refreshable, IMapleLog log)
        {
            _refreshable = refreshable ?? throw new ArgumentNullException(nameof(refreshable));
            _log = log ?? throw new ArgumentNullException(nameof(log));
        }

        public void Load()
        {
            _log.Info($"{Resources.Loading} {_refreshable.GetType().Name}");
            _refreshable.Load();
        }

        public void Save()
        {
            _log.Info($"{Resources.Saving} {_refreshable.GetType().Name}");
            _refreshable.Save();
        }
    }
}
