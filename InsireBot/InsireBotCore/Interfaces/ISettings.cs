using System.IO;

namespace InsireBotCore
{
    public interface ISettings
    {
        string FileName { get; set; }
        DirectoryInfo Directory { get; set; }
        RepeatMode RepeatMode { get; set; }
        string[] Options { get; set; }
    }
}
