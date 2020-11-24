using Prism.Mvvm;

namespace RPGDataEditor.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string title = "RPG Data Editor";
        public string Title {
            get => title;
            set => SetProperty(ref title, value);
        }
    }
}
