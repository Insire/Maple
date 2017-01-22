namespace InsireBot.Core
{
    /// <summary>
    ///  provides a property for an instance of a class to tell, if the instance has been selected via the UI
    /// </summary>
    public interface IIsSelected
    {
        bool IsSelected { get; set; }
    }
}
