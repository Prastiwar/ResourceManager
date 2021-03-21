using Prism.Commands;
using RPGDataEditor.Core.Models;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RPGDataEditor.Core.Mvvm
{
    public abstract class SimpleCategorizedTabViewModel<TModel> : SimpleIdentifiableTabViewModel<TModel>, ICategorizedTabViewModel where TModel : IdentifiableData
    {
        public SimpleCategorizedTabViewModel(ViewModelContext context,
                                             ITypeToResourceConverter resourceConverter,
                                             ILocationToSimpleResourceConverter simpleResourceConverter) 
            : base(context, resourceConverter, simpleResourceConverter) { }

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

        private async void RemoveCategory(string category) => await RemoveCategoryAsync(category);

        public override async Task Refresh()
        {
            Categories = new ObservableCollection<string>();
            await base.Refresh();
            Categories.AddRange(Models.Select(x => (x as SimpleCategorizedData).Category).Distinct());
        }

        protected override Task OnSavingAsync(SimpleIdentifiableData model, EditorResults results)
        {
            if (results.Success)
            {
                TModel exactModel = (TModel)results.Model;
                (model as SimpleCategorizedData).Name = exactModel.Title;
            }
            return Task.CompletedTask;
        }

        protected override async Task<SimpleIdentifiableData> CreateModelAsync()
        {
            SimpleIdentifiableData newModel = await base.CreateModelAsync();
            if (newModel != null)
            {
                SimpleCategorizedData categorizedData = new SimpleCategorizedData(typeof(TModel)) {
                    Name = newModel.Name,
                    Id = newModel.Id,
                    Category = CurrentCategory
                };
                Models.Add(categorizedData);
                return categorizedData;
            }
            return null;
        }

        protected override SimpleIdentifiableData CreateModelInstance() => new SimpleCategorizedData(typeof(TModel));

        protected virtual void ShowCategory(string category) => CurrentCategory = category;

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

        public virtual async Task<bool> RenameCategoryAsync(string oldCategory, string newCategory)
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
                try
                {
                    (model as SimpleCategorizedData).Category = newCategory;
                    TModel oldModel = (TModel)await Session.Client.GetAsync(model);
                    TModel newModel = oldModel;
                    newModel.Category = newCategory;

                    bool saved = await Session.Client.UpdateAsync(oldModel, newModel);
                    if (!saved)
                    {
                        Context.SnackbarService.Enqueue("Couldn't save model " + model.Name);
                    }
                }
                catch (System.Exception ex)
                {
                    Logger.Error("One of models couldn't be renamed", ex);
                    Context.SnackbarService.Enqueue("Model couldn't be renamed " + model.Name);
                }
            }
            return true;
        }

        public virtual async Task<bool> RemoveCategoryAsync(string category)
        {
            bool removed = Categories.Remove(category);
            if (removed)
            {
                foreach (SimpleIdentifiableData model in Models.Where(x => string.Compare((x as SimpleCategorizedData).Category, category) == 0))
                {
                    bool deleted = await Session.Client.DeleteAsync(model);
                    if (!deleted)
                    {
                        Context.SnackbarService.Enqueue("Couldn't delete model " + model.Name);
                    }
                }
            }
            return removed;
        }

    }
}
