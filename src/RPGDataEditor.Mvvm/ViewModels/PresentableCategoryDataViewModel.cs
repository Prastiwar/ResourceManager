using MediatR;
using Microsoft.Extensions.Logging;
using ResourceManager;
using ResourceManager.Commands;
using ResourceManager.Data;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace RPGDataEditor.Mvvm
{
    public abstract class PresentableCategoryDataViewModel<TResource> : PresentableDataViewModel<TResource> where TResource : IIdentifiable
    {
        public PresentableCategoryDataViewModel(IMediator mediator, ILogger<PresentableCategoryDataViewModel<TResource>> logger) : base(mediator, logger) { }

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

        public virtual async Task<bool> RenameCategoryAsync(string oldCategory, string newCategory)
        {
            bool categoryExists = Categories.IndexOf(newCategory) != -1;
            if (categoryExists)
            {
                return false;
            }
            int oldCategoryIndex = Categories.IndexOf(oldCategory);
            bool removed = Categories.Remove(oldCategory);
            if (!removed)
            {
                return false;
            }
            RenameCategoryResults results = await Mediator.Send(new RenameCategoryRequest(typeof(TResource), default, oldCategory, newCategory));
            if (!results.IsSuccess)
            {
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
            bool removed = Categories.Remove(category);
            if (removed)
            {
                RemoveCategoryResults results = await Mediator.Send(new RemoveCategoryRequest(typeof(TResource), category));
                return results.IsSuccess;
            }
            return removed;
        }

    }
}
