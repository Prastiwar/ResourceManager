namespace RPGDataEditor.Core
{
    public class AppVersion
    {
        public AppVersion(string version) => Version = version;

        public string Version { get; set; }

        public override string ToString() => Version;
    }
}
