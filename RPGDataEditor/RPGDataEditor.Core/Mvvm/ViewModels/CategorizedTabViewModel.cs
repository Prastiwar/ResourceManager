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

        private ObservableCollection<TModel> currentCategoryModels;
        public ObservableCollection<TModel> CurrentCategoryModels {
            get => currentCategoryModels;
            protected set => SetProperty(ref currentCategoryModels, value);
        }

        private ObservableCollection<string> modelCategories;
        public ObservableCollection<string> ModelCategories {
            get => modelCategories;
            protected set => SetProperty(ref modelCategories, value);
        }

        private ICommand addCategoryCommand;
        public ICommand AddCategoryCommand => addCategoryCommand ??= new DelegateCommand(CreateCategory);

        private ICommand removeCategoryCommand;
        public ICommand RemoveCategoryCommand => removeCategoryCommand ??= new DelegateCommand<string>(RemoveCategory);

        private ICommand showCategoryCommand;
        public ICommand ShowCategoryCommand => showCategoryCommand ??= new DelegateCommand<string>(ShowCategory);

        public override async Task Refresh()
        {
            ModelCategories = new ObservableCollection<string>();
            await base.Refresh();
            ModelCategories.AddRange(Models.Select(x => x.Category).Distinct());
        }

        protected virtual void ShowCategory(string category)
        {
            CurrentCategoryModels = new ObservableCollection<TModel>();
            CurrentCategoryModels.AddRange(Models.Where(x => string.Compare(x.Category, category) == 0));
        }

        protected override async Task<TModel> CreateModelAsync()
        {
            TModel newModel = await base.CreateModelAsync();
            if (newModel != null)
            {
                newModel.Title = "New model";
                newModel.Category = "Uncategorized"; // TODO: there should be actual category
                CurrentCategoryModels.Add(newModel);
            }
            return newModel;
        }

        protected override async Task<bool> RemoveModelAsync(TModel model)
        {
            bool removed = await base.RemoveModelAsync(model);
            if (removed)
            {
                CurrentCategoryModels.Remove(model);
            }
            return removed;
        }

        protected void CreateCategory()
        {
            string newName = "New Category";
            int i = 1;
            while (ModelCategories.IndexOf(newName) != -1)
            {
                newName = $"New Category ({i})";
                i++;
            }
            ModelCategories.Add(newName);
        }

        public async Task<bool> RenameCategoryAsync(string oldCategory, string newCategory)
        {
            if (ModelCategories.IndexOf(newCategory) != -1)
            {
                return false;
            }
            int oldCategoryIndex = ModelCategories.IndexOf(oldCategory);
            bool removed = ModelCategories.Remove(oldCategory);
            if (!removed)
            {
                return false;
            }
            ModelCategories.Insert(oldCategoryIndex, newCategory);
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
            bool removed = ModelCategories.Remove(category);
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
