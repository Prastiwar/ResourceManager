using Newtonsoft.Json;
using Prism.Commands;
using RPGDataEditor.Core.Models;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RPGDataEditor.Core.Mvvm
{
    public abstract class SimpleCategorizedTabViewModel<TModel> : SimpleIdentifiableTabViewModel<TModel>, ICategorizedTabViewModel where TModel : IdentifiableData
    {
        public SimpleCategorizedTabViewModel(ViewModelContext context) : base(context) { }

        private ICommand addCategoryCommand;
        public ICommand AddCategoryCommand => addCategoryCommand ??= new DelegateCommand(CreateCategory);

        private ICommand removeCategoryCommand;
        public ICommand RemoveCategoryCommand => removeCategoryCommand ??= new DelegateCommand<string>(RemoveCategory);

        private ICommand showCategoryCommand;
        public ICommand ShowCategoryCommand => showCategoryCommand ??= new DelegateCommand<string>(ShowCategory);

        private ObservableCollection<string> categories;
        public ObservableCollection<string> Categories {
            get => categories;
            protected set => SetProperty(ref categories, value);
        }

        private string currentCategory;
        public string CurrentCategory {
            get => currentCategory;
            protected set => SetProperty(ref currentCategory, value);
        }

        public override async Task Refresh()
        {
            Categories = new ObservableCollection<string>();
            Models = new ObservableCollection<SimpleIdentifiableData>();
            IsLoading = true;
            try
            {
                string[] files = await Session.GetFilesAsync(RelativePath);
                foreach (string file in files)
                {
                    SimpleIdentifiableData newModel = await CreateSimpleCategorizedModelAsync(file);
                    Models.Add(newModel);
                }
                Categories.AddRange(Models.Select(x => (x as SimpleCategorizedData).Category).Distinct());
            }
            catch (Exception ex)
            {
                Logger.Error("Failed to load files, class: " + GetType().Name, ex);
                Context.SnackbarService.Enqueue("Failed to load files, you can try again by refreshing tab");
            }
            IsLoading = false;
        }

        protected virtual void ShowCategory(string category) => CurrentCategory = category;

        protected override async Task<SimpleIdentifiableData> CreateModelAsync()
        {
            SimpleIdentifiableData newModel = await base.CreateModelAsync();
            if (newModel != null)
            {
                SimpleCategorizedData categorizedData = new SimpleCategorizedData() {
                    Name = newModel.Name,
                    Id = newModel.Id,
                    Category = CurrentCategory
                };
                Models.Add(categorizedData);
            }
            return null;
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
            foreach (SimpleIdentifiableData model in Models.Where(x => string.Compare((x as SimpleCategorizedData).Category, oldCategory) == 0))
            {
                string relativeFilePath = GetRelativeFilePath(model);
                (model as SimpleCategorizedData).Category = newCategory;
                //string json = JsonConvert.SerializeObject(model);
                //bool saved = await Session.SaveJsonFileAsync(relativeFilePath, json);
            }
            return true;
        }

        protected async void RemoveCategory(string category)
        {
            bool removed = Categories.Remove(category);
            if (removed)
            {
                foreach (SimpleIdentifiableData model in Models.Where(x => string.Compare((x as SimpleCategorizedData).Category, category) == 0))
                {
                    string relativeFilePath = GetRelativeFilePath(model);
                    bool deleted = await Session.DeleteFileAsync(relativeFilePath);
                }
            }
        }

        protected virtual async Task<SimpleIdentifiableData> CreateSimpleCategorizedModelAsync(string file)
        {
            SimpleIdentifiableData data = CreateSimpleModel(file);
            string relativePath = Path.GetRelativePath(Session.LocationPath, file);
            string json = await Session.GetJsonAsync(relativePath);
            string categoryJson = "category\":";
            int categoryIndex = json.IndexOf(categoryJson);
            int categoryValueStartIndex = json.IndexOf('"', categoryIndex + categoryJson.Length) + 1;
            int categoryValueEndIndex = json.IndexOf('"', categoryValueStartIndex);
            string category = json[categoryValueStartIndex..categoryValueEndIndex];
            return new SimpleCategorizedData() {
                Id = data.Id,
                Name = data.Name,
                Category = category
            };
        }

        protected sealed override SimpleIdentifiableData CreateSimpleModel(string file) => base.CreateSimpleModel(file);

        protected override SimpleIdentifiableData CreateModelInstance() => new SimpleCategorizedData();

    }
}
