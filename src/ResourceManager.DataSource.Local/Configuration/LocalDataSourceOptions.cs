using System.IO;

namespace ResourceManager.DataSource.Local.Configuration
{
    public class LocalDataSourceOptions
    {
        public string FolderPath { get; set; }

        public string FileSearchPattern { get; set; }

        public string GetFullFolderPath() => Path.GetFullPath(FolderPath);
    }
}
