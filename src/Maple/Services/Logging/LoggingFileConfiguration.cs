namespace Maple
{
    public sealed class LoggingFileConfiguration
    {
        internal const long DefaultFileSizeLimitBytes = 1024 * 1024 * 1024;
        internal const int DefaultRetainedFileCountLimit = 28;

        /// <summary>
        /// Filname to write. The filename may include <c>{Date}</c> to specify
        /// how the date portion of the filename is calculated. May include
        /// environment variables.
        /// </summary>
        public string PathFormat { get; set; }

        /// <summary>
        /// The maximum size, in bytes, to which any single log file will be
        /// allowed to grow. For unrestricted growth, pass <c>null</c>. The
        /// default is 1 GiB.
        /// </summary>
        public long? FileSizeLimitBytes { get; set; }

        /// <summary>
        /// The maximum number of log files that will be retained, including
        /// the current log file. For unlimited retention, pass <c>null</c>.
        /// The default is 31.
        /// </summary>
        public int? RetainedFileCountLimit { get; set; }
    }
}
