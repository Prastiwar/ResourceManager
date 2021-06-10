﻿using Microsoft.Extensions.Logging;
using ResourceManager;
using ResourceManager.Data;
using ResourceManager.DataSource;
using RPGDataEditor.Mvvm.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace RPGDataEditor.Mvvm
{
    public abstract class PresentableCategoryDataViewModel<TResource> : PresentableDataViewModel<TResource> where TResource : IIdentifiable
    {
        public PresentableCategoryDataViewModel(IViewService viewService, IDataSource dataSource, ILogger<PresentableCategoryDataViewModel<TResource>> logger)
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
            Categories.AddRange(Models.Select(x => (x as PresentableCategoryData).Category).Distinct());
        }

        protected override PresentableData CreateModelInstance() => new PresentableCategoryData(typeof(TResource)) { Category = CurrentCategory };

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

        protected override PresentableData CreatePresentableData(string location)
        {
            PresentableCategoryData presentable = (PresentableCategoryData)CreateModelInstance();
            LocationResourceDescriptor pathDescriptor = DataSource.DescriptorService.GetRequiredDescriptor<LocationResourceDescriptor>(typeof(TResource));
            KeyValuePair<string, object>[] parameters = pathDescriptor.ParseParameters(location);
            presentable.Id = parameters.FirstOrDefault(x => string.Compare(x.Key, nameof(PresentableData.Id), true) == 0).Value;
            presentable.Name = parameters.FirstOrDefault(x => string.Compare(x.Key, nameof(PresentableData.Name), true) == 0).Value?.ToString();
            presentable.Category = parameters.FirstOrDefault(x => string.Compare(x.Key, nameof(PresentableCategoryData.Category), true) == 0).Value?.ToString();
            return presentable;
        }

        public virtual async Task<bool> RenameCategoryAsync(string oldCategory, string newCategory)
        {
            throw new System.NotImplementedException();
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
            List<TResource> resources = null;// DataSource.Query<TResource>().Where(x => EqualityComparer<string>.Default.Equals(x.Category, oldCategory)).ToList();
            foreach (TResource resource in resources)
            {
                //resource.Category = newCategory;
                await DataSource.UpdateAsync(resource);
            }
            try
            {
                await DataSource.SaveChangesAsync();
            }
            catch (System.Exception ex)
            {
                Categories.Insert(oldCategoryIndex, oldCategory);
                Logger.LogError(ex, "Couldn't rename category {0} to {1}", oldCategory, newCategory);
                return false;
            }
            Categories.Insert(oldCategoryIndex, newCategory);
            foreach (PresentableCategoryData model in Models.Cast<PresentableCategoryData>().Where(x => string.Compare(x.Category, oldCategory) == 0))
            {
                model.Category = newCategory;
            }
            return true;
        }

        public virtual async Task<bool> RemoveCategoryAsync(string category)
        {
            throw new System.NotImplementedException();
            bool removed = Categories.Remove(category);
            if (removed)
            {
                List<TResource> resources = null;// DataSource.Query<TResource>().Where(x => EqualityComparer<string>.Default.Equals(x.Category, oldCategory)).ToList();
                foreach (TResource resource in resources)
                {
                    await DataSource.DeleteAsync(resource);
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
