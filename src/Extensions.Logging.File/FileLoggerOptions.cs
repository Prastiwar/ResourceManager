using System;

namespace Extensions.Logging.File
{
    public class FileLoggerOptions
    {
        public Func<string> FilePathFunc { get; set; }
        public int MaxFilesCount { get; set; }
        public int MaxFileSize { get; set; }
    }
}
