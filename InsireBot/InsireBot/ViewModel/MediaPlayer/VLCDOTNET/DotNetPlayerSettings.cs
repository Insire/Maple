using System.IO;

namespace InsireBot
{
    public class DotNetPlayerSettings : ISettings
    {
        public DirectoryInfo Directory { get; set; }
        public string FileName { get; set; }
        public string[] Options { get; set; }
        public RepeatMode RepeatMode { get; set; }
    }
}
