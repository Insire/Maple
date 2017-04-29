using Maple.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Maple.Core
{
    /// <summary>
    /// ListViewModel implementation for ObservableObjects related to the DataAccessLayer (DB)
    /// </summary>
    /// <typeparam name="T">a wrapper class implementing <see cref="BaseViewModel" /></typeparam>
    /// <typeparam name="S">a DTO implementing <see cref="BaseObject" /></typeparam>
    /// <seealso cref="Maple.Core.BaseListViewModel{T}" />
    public abstract class BaseDataListViewModel<T, S> : BaseListViewModel<T>, ILoadableViewModel, IDisposable where T : BaseViewModel<S>, ISequence where S : BaseObject
    {
        protected readonly ISequenceProvider _sequenceProvider;
        protected readonly ITranslationService _translationService;
        protected readonly IMapleLog _log;

        private bool _disposed;
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="MediaPlayers"/> is disposed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if disposed; otherwise, <c>false</c>.
        /// </value>
        public bool Disposed
        {
            get { return _disposed; }
            protected set { SetValue(ref _disposed, value); }
        }

        /// <summary>
        /// Gets the load command.
        /// </summary>
        /// <value>
        /// The load command.
        /// </value>
        public ICommand LoadCommand => new RelayCommand(Load, () => !IsLoaded);
        /// <summary>
        /// Gets the refresh command.
        /// </summary>
        /// <value>
        /// The refresh command.
        /// </value>
        public ICommand RefreshCommand => new RelayCommand(Load);
        /// <summary>
        /// Gets the save command.
        /// </summary>
        /// <value>
        /// The save command.
        /// </value>
        public ICommand SaveCommand => new RelayCommand(Save);
        /// <summary>
        /// Gets a value indicating whether this instance is loaded.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is loaded; otherwise, <c>false</c>.
        /// </value>
        public bool IsLoaded { get; protected set; }

        public BaseDataListViewModel(IMapleLog log, ITranslationService translationService, ISequenceProvider sequenceProvider)
        {
            _log = log ?? throw new ArgumentNullException(nameof(log));
            _translationService = translationService ?? throw new ArgumentNullException(nameof(translationService));
            _sequenceProvider = sequenceProvider ?? throw new ArgumentNullException(nameof(sequenceProvider));
        }
        public abstract void Add();
        public abstract void Load();
        public abstract void Save();
        public abstract Task LoadAsync();
        public abstract Task SaveAsync();

        /// <summary>
        /// Removes the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        public override void Remove(T item)
        {
            item.Model.IsDeleted = true;
            base.Remove(item);
        }

        /// <summary>
        /// Removes the range.
        /// </summary>
        /// <param name="items">The items.</param>
        public override void RemoveRange(IEnumerable<T> items)
        {
            base.RemoveRange(items);

            items.ForEach(p => p.Model.IsDeleted = true);
        }

        public override void Add(T item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            var sequence = _sequenceProvider.Get(Items.Cast<ISequence>().ToList());
            item.Sequence = sequence;
            base.Add(item);
        }

        public override void AddRange(IEnumerable<T> items)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            using (BusyStack.GetToken())
            {
                var added = false;
                var sequence = _sequenceProvider.Get(Items.Cast<ISequence>().ToList());

                foreach (var item in items)
                {
                    item.Sequence = sequence;
                    Add(item);

                    sequence++;
                    added = true;
                }

                if (SelectedItem == null && added)
                    SelectedItem = Items.First();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (Disposed)
                return;

            if (disposing)
            {
                // Free any other managed objects here.
            }

            // Free any unmanaged objects here.
            Disposed = true;
        }
    }
}
