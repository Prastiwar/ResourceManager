namespace RPGDataEditor.Core.Models
{
    public abstract class NpcJobModel : ObservableModel
    {
        public string Type => GetType().Name.Replace("NpcJob", "");
    }
}
