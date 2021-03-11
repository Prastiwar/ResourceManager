using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RPGDataEditor.Core.Mvvm
{
    public interface ICategorizedTabViewModel
    {
        public ObservableCollection<string> Categories { get; }

        Task<bool> RenameCategoryAsync(string oldCategory, string newCategory);

        ICommand AddCategoryCommand { get; }

        ICommand RemoveCategoryCommand { get; }

        ICommand ShowCategoryCommand { get; }
    }
}
