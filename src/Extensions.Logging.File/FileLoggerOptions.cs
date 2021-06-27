using System;

namespace Extensions.Logging.File
{

    public class FileLoggerOptions
    {
        public int MaxFilesCount { get; set; }
        public long MaxFileSize { get; set; }
        public Func<string> FilePathFunc { get; set; }
        public BuildLogMessageHandler LogMessageBuilder { get; set; }
    }
}
