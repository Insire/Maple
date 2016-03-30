using System.IO;

namespace InsireBot.MediaPlayer
{
    public class DotNetPlayerSettings : ISettings
    {
        public string Directory { get; set; }
        public string Extension { get; set; }
        public string FileName { get; set; }
        public DirectoryInfo VlcLibDirectory { get; set; }
        public string[] Options { get; set; }
        public RepeatMode RepeatMode { get; set; }
    }
}
