namespace RPGDataEditor.Mvvm.Models
{
    public class Position : ObservableModel
    {
        private int x;
        public int X {
            get => x;
            set => SetProperty(ref x, value);

        }

        private int y;
        public int Y {
            get => y;
            set => SetProperty(ref y, value);

        }

        private int z;
        public int Z {
            get => z;
            set => SetProperty(ref z, value);

        }
    }
}
