using AutoUpdaterDotNET;

namespace RPGDataEditor.Wpf
{
    public class UpdateInfo
    {
        public string Version { get; set; }
        public string Url { get; set; }
        public string Changelog { get; set; }
        public string Args { get; set; }
        public Mandatory Mandatory { get; set; }
        public CheckSum CheckSum { get; set; }
    }
}
