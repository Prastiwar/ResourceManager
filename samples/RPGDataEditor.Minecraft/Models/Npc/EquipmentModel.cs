using RPGDataEditor.Models;

namespace RPGDataEditor.Minecraft.Models
{
    public class Equipment : ObservableModel
    {
        private string chest = "";
        public string Chest {
            get => chest;
            set => SetProperty(ref chest, value ?? "");
        }

        private string head = "";
        public string Head {
            get => head;
            set => SetProperty(ref head, value ?? "");
        }

        private string mainHand = "";
        public string MainHand {
            get => mainHand;
            set => SetProperty(ref mainHand, value ?? "");
        }

        private string offHand = "";
        public string OffHand {
            get => offHand;
            set => SetProperty(ref offHand, value ?? "");
        }

        private string feet = "";
        public string Feet {
            get => feet;
            set => SetProperty(ref feet, value ?? "");
        }

        private string legs = "";
        public string Legs {
            get => legs;
            set => SetProperty(ref legs, value ?? "");
        }
    }
}
