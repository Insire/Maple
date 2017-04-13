namespace Maple.Core
{
    /// <summary>
    /// provides a property for an instance of a class to tell, if the instance has been selected via the UI
    /// </summary>
    public interface IIsSelected
    {
        /// <summary>
        /// Gets or sets a value indicating whether this instance is selected.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is selected; otherwise, <c>false</c>.
        /// </value>
        bool IsSelected { get; set; }
    }
}
