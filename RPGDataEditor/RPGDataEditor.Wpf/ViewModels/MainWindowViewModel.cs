using RPGDataEditor.Core.Mvvm;

namespace RPGDataEditor.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel(ViewModelContext context) : base(context) { }

        private string title = "RPG Data Editor";
        public string Title {
            get => title;
            set => SetProperty(ref title, value);
        }
    }
}
