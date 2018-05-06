﻿namespace Maple.Domain
{
    public interface IChangeState
    {
        /// <summary>
        /// Gets a value indicating whether this instance is new.
        /// </summary>
        /// <value><c>true</c> if this instance is new; otherwise, <c>false</c>.</value>
        bool IsNew { get; }
    }
}
