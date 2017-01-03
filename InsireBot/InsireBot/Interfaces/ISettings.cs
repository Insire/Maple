using System.IO;

namespace InsireBot
{
    public interface ISettings
    {
        string FileName { get; set; }
        DirectoryInfo Directory { get; set; }
        RepeatMode RepeatMode { get; set; }
        string[] Options { get; set; }
    }
}
