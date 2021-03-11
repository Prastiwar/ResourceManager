using Newtonsoft.Json;
using Prism.Commands;
using RPGDataEditor.Core.Models;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RPGDataEditor.Core.Mvvm
{
    public abstract class CategorizedTabViewModel<TModel> : IdentifiableTabViewModel<TModel>, ICategorizedTabViewModel where TModel : IdentifiableData
    {
        public CategorizedTabViewModel(ViewModelContext context) : base(context) { }

        private string currentCategory;
        public string CurrentCategory {
            get => currentCategory;
            protected set => SetProperty(ref currentCategory, value);
        }

        private ObservableCollection<string> categories;
        public ObservableCollection<string> Categories {
            get => categories;
            protected set => SetProperty(ref categories, value);
        }

        private ICommand addCategoryCommand;
        public ICommand AddCategoryCommand => addCategoryCommand ??= new DelegateCommand(CreateCategory);

        private ICommand removeCategoryCommand;
        public ICommand RemoveCategoryCommand => removeCategoryCommand ??= new DelegateCommand<string>(RemoveCategory);

        private ICommand showCategoryCommand;
        public ICommand ShowCategoryCommand => showCategoryCommand ??= new DelegateCommand<string>(ShowCategory);

        public override async Task Refresh()
        {
            Categories = new ObservableCollection<string>();
            await base.Refresh();
            Categories.AddRange(Models.Select(x => x.Category).Distinct());
        }

        protected virtual void ShowCategory(string category) => CurrentCategory = category;

        protected override async Task<TModel> CreateModelAsync()
        {
            TModel newModel = await base.CreateModelAsync();
            if (newModel != null)
            {
                newModel.Title = "New model";
                newModel.Category = CurrentCategory;
            }
            return newModel;
        }

        protected void CreateCategory()
        {
            string newName = "New Category";
            int i = 1;
            while (Categories.IndexOf(newName) != -1)
            {
                newName = $"New Category ({i})";
                i++;
            }
            Categories.Add(newName);
        }

        public async Task<bool> RenameCategoryAsync(string oldCategory, string newCategory)
        {
            if (Categories.IndexOf(newCategory) != -1)
            {
                return false;
            }
            int oldCategoryIndex = Categories.IndexOf(oldCategory);
            bool removed = Categories.Remove(oldCategory);
            if (!removed)
            {
                return false;
            }
            Categories.Insert(oldCategoryIndex, newCategory);
            foreach (TModel model in Models.Where(x => string.Compare(x.Category, oldCategory) == 0))
            {
                string relativeFilePath = GetRelativeFilePath(model);
                model.Category = newCategory;
                string json = JsonConvert.SerializeObject(model);
                bool saved = await Session.SaveJsonFileAsync(relativeFilePath, json);
            }
            return true;
        }

        protected async void RemoveCategory(string category)
        {
            bool removed = Categories.Remove(category);
            if (removed)
            {
                foreach (TModel model in Models.Where(x => string.Compare(x.Category, category) == 0))
                {
                    string relativeFilePath = GetRelativeFilePath(model);
                    bool deleted = await Session.DeleteFileAsync(relativeFilePath);
                }
            }
        }
    }
}
