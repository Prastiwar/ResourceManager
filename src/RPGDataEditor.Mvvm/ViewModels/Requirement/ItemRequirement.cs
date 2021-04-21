namespace RPGDataEditor.Mvvm.Models
{
    public class ItemRequirement : PlayerRequirementModel
    {
        private string item;
        public string Item {
            get => item;
            set => SetProperty(ref item, value);
        }
    }
}
