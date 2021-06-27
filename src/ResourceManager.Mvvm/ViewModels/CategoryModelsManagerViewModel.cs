using Microsoft.Extensions.Logging;
using ResourceManager;
using ResourceManager.Data;
using ResourceManager.DataSource;
using ResourceManager.Mvvm.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace ResourceManager.Mvvm
{
    public abstract class CategoryModelsManagerViewModel<TResource> : ModelsManagerViewModel<TResource> where TResource : ICategorizable
    {
        public CategoryModelsManagerViewModel(IViewService viewService, IDataSource dataSource, ILogger<CategoryModelsManagerViewModel<TResource>> logger)
            : base(viewService, dataSource, logger) { }

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
            await base.Refresh();
            Categories.AddRange(Models.Select(x => x.Category).Distinct());
        }

        protected virtual void ShowCategory(string category) => CurrentCategory = category;

        protected string CreateCategory()
        {
            string newName = "New Category";
            int i = 1;
            while (Categories.IndexOf(newName) != -1)
            {
                newName = $"New Category ({i})";
                i++;
            }
            Categories.Add(newName);
            return newName;
        }

        public virtual async Task<bool> RenameCategoryAsync(string oldCategory, string newCategory)
        {
            bool categoryExists = Categories.IndexOf(newCategory) != -1;
            if (categoryExists)
            {
                return false;
            }
            int oldCategoryIndex = Categories.IndexOf(oldCategory);
            if (oldCategoryIndex < 0)
            {
                return false;
            }
            Categories.RemoveAt(oldCategoryIndex);
            Categories.Insert(oldCategoryIndex, newCategory);
            IEnumerable<TResource> foundModels = Models.Where(x => string.Compare(x.Category, oldCategory) == 0);
            foreach (TResource model in foundModels)
            {
                model.Category = newCategory;
                await DataSource.UpdateAsync(model);
            }
            try
            {
                await DataSource.SaveChangesAsync();
            }
            catch (System.Exception ex)
            {
                foreach (TResource model in foundModels)
                {
                    model.Category = oldCategory;
                }
                Categories.RemoveAt(oldCategoryIndex);
                Categories.Insert(oldCategoryIndex, oldCategory);
                Logger.LogError(ex, "Couldn't rename category {0} to {1}", oldCategory, newCategory);
                return false;
            }
            return true;
        }

        public virtual async Task<bool> RemoveCategoryAsync(string category)
        {
            bool removed = Categories.Remove(category);
            if (removed)
            {
                IEnumerable<TResource> foundModels = Models.Where(x => string.Compare(x.Category, category) == 0);
                foreach (TResource model in foundModels)
                {
                    await DataSource.DeleteAsync(model);
                }
                try
                {
                    await DataSource.SaveChangesAsync();
                }
                catch (System.Exception ex)
                {
                    Categories.Add(category);
                    Logger.LogError(ex, "Couldn't remove category: " + category);
                    return false;
                }
            }
            return removed;
        }

    }
}
