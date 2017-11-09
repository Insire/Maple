using System.Diagnostics.CodeAnalysis;

namespace Maple.Core
{
    /// <summary>
    /// Enum representing the possible types of a sequence.
    /// </summary>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public enum SequenceType
    {
        /// <summary>
        /// Represents an invalid type.
        /// </summary>
        Invalid = 0,

        /// <summary>
        /// Represents a <see cref="String"/>.
        /// </summary>
        String,

        /// <summary>
        /// Represents an Array.
        /// </summary>
        Array,

        /// <summary>
        /// Represents a <see cref="BitArray"/>. This type is non generic.
        /// </summary>
        BitArray,

        /// <summary>
        /// Represents an <see cref="ArrayList"/>. This type is non generic.
        /// </summary>
        ArrayList,

        /// <summary>
        /// Represents a <see cref="Queue"/>. This type is non generic.
        /// </summary>
        Queue,

        /// <summary>
        /// Represents a <see cref="Stack"/>. This type is non generic.
        /// </summary>
        Stack,

        /// <summary>
        /// Represents a <see cref="Hashtable"/>. This type is non generic.
        /// </summary>
        Hashtable,

        /// <summary>
        /// Represents a <see cref="SortedList"/>. This type is non generic.
        /// </summary>
        SortedList,

        /// <summary>
        /// Represents a <see cref="Dictionary"/>. This type is non generic.
        /// </summary>
        Dictionary,

        /// <summary>
        /// Represents a <see cref="ListDictionary"/>. This type is non generic.
        /// </summary>
        ListDictionary,

        /// <summary>
        /// Represents an <see cref="IList"/>. This interface type is non generic.
        /// </summary>
        IList,

        /// <summary>
        /// Represents an <see cref="ICollection"/>. This interface type is non generic.
        /// </summary>
        ICollection,

        /// <summary>
        /// Represents an <see cref="IDictionary"/>. This interface type is non generic.
        /// </summary>
        IDictionary,

        /// <summary>
        /// Represents an <see cref="IEnumerable"/>. This interface type is non generic.
        /// </summary>
        IEnumerable,

        /// <summary>
        /// Represents a custom implementation of <see cref="IEnumerable"/>.
        /// </summary>
        Custom,

        /// <summary>
        /// Represents a <see cref="List{T}"/>.
        /// </summary>
        GenericList,

        /// <summary>
        /// Represents a <see cref="LinkedList{T}"/>.
        /// </summary>
        GenericLinkedList,

        /// <summary>
        /// Represents a <see cref="Collection{T}"/>.
        /// </summary>
        GenericCollection,

        /// <summary>
        /// Represents a <see cref="Queue{T}"/>.
        /// </summary>
        GenericQueue,

        /// <summary>
        /// Represents a <see cref="Stack{T}"/>.
        /// </summary>
        GenericStack,

        /// <summary>
        /// Represents a <see cref="HashSet{T}"/>.
        /// </summary>
        GenericHashSet,

        /// <summary>
        /// Represents a <see cref="SortedList{TKey,TValue}"/>.
        /// </summary>
        GenericSortedList,

        /// <summary>
        /// Represents a <see cref="Dictionary{TKey,TValue}"/>.
        /// </summary>
        GenericDictionary,

        /// <summary>
        /// Represents a <see cref="SortedDictionary{TKey, TValue}"/>.
        /// </summary>
        GenericSortedDictionary,

        /// <summary>
        /// Represents a <see cref="BlockingCollection{T}"/>.
        /// </summary>
        GenericBlockingCollection,

        /// <summary>
        /// Represents a <see cref="ConcurrentDictionary{TKey, TValue}"/>.
        /// </summary>
        GenericConcurrentDictionary,

        /// <summary>
        /// Represents a <see cref="ConcurrentBag{T}"/>.
        /// </summary>
        GenericConcurrentBag,

        /// <summary>
        /// Represents an <see cref="IList{T}"/>.
        /// </summary>
        GenericIList,

        /// <summary>
        /// Represents an <see cref="ICollection{T}"/>.
        /// </summary>
        GenericICollection,

        /// <summary>
        /// Represents an <see cref="IEnumerable{T}"/>.
        /// </summary>
        GenericIEnumerable,

        /// <summary>
        /// Represents an <see cref="IDictionary{TKey, TValue}"/>.
        /// </summary>
        GenericIDictionary,

        /// <summary>
        /// Represents an <see> <cref>ICollection{KeyValuePair{TKey, TValue}}</cref></see>.
        /// </summary>
        GenericICollectionKeyValue,

        /// <summary>
        /// Represents an <see> <cref>IEnumerable{KeyValuePair{TKey, TValue}}</cref></see>.
        /// </summary>
        GenericIEnumerableKeyValue,

        /// <summary>
        /// Represents a custom implementation of <see cref="IEnumerable{T}"/>.
        /// </summary>
        GenericCustom
    }
}
