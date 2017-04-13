namespace Maple.Core
{
    /// <summary>
    /// Provides an index property which is meant for managing instances of the class inside of a collection
    /// </summary>
    public interface ISequence
    {
        /// <summary>
        /// Gets or sets the sequence.
        /// </summary>
        /// <value>
        /// The sequence.
        /// </value>
        int Sequence { get; set; }
    }
}
