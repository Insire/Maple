using NAudio.Wave;
using System.IO;

namespace InsireBot.NAudio
{
    public class FormatFactory
    {
        public static WaveFormat GetWaveFormat(string fileName)
        {
            using (var reader = new WaveFileReader(fileName))
            {
                return reader.WaveFormat;
            }
        }

        public static WaveFormat GetWaveFormat(Stream stream)
        {
            using (var reader = new WaveFileReader(stream))
            {
                return reader.WaveFormat;
            }
        }
    }
}
