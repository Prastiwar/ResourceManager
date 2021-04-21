namespace RPGDataEditor.Mvvm.Models
{
    public class MoneyRequirement : PlayerRequirementModel
    {
        private int money;
        public int Money {
            get => money;
            set => SetProperty(ref money, value);
        }
    }
}
