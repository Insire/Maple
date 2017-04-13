using System;
using System.ComponentModel;

namespace Maple.Core
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Maple.Core.IIsSelected" />
    /// <seealso cref="Maple.Core.ISequence" />
    /// <seealso cref="System.ComponentModel.INotifyPropertyChanged" />
    /// <seealso cref="Maple.Core.IIdentifier" />
    /// <seealso cref="Maple.Core.IChangeState" />
    public interface IMediaItem : IIsSelected, ISequence, INotifyPropertyChanged, IIdentifier, IChangeState
    {
        /// <summary>
        /// Gets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        string Title { get; }
        /// <summary>
        /// Gets the location.
        /// </summary>
        /// <value>
        /// The location.
        /// </value>
        string Location { get; }

        /// <summary>
        /// Gets the duration.
        /// </summary>
        /// <value>
        /// The duration.
        /// </value>
        TimeSpan Duration { get; }

        /// <summary>
        /// Gets the privacy status.
        /// </summary>
        /// <value>
        /// The privacy status.
        /// </value>
        PrivacyStatus PrivacyStatus { get; }
    }
}
