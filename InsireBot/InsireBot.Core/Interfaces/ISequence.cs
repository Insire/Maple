namespace InsireBot.Core
{
    /// <summary>
    /// Provides an index property which is meant for managing instances of the class inside of a collection
    /// </summary>
    public interface ISequence
    {
        int Sequence{ get; set; }
    }
}
