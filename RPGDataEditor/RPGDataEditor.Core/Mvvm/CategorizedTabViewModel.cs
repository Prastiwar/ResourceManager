using Newtonsoft.Json;
using Prism.Commands;
using Prism.Regions;
using RPGDataEditor.Core.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RPGDataEditor.Core.Mvvm
{
    public abstract class CategorizedTabViewModel<TModel> : TabViewModel, ICategorizedTabViewModel where TModel : IdentifiableData
    {
        public CategorizedTabViewModel(ViewModelContext context) : base(context) { }

        private ObservableCollection<TModel> currentCategoryModels;
        public ObservableCollection<TModel> CurrentCategoryModels {
            get => currentCategoryModels;
            protected set => SetProperty(ref currentCategoryModels, value);
        }

        private ObservableCollection<TModel> models;
        public ObservableCollection<TModel> Models {
            get => models;
            protected set => SetProperty(ref models, value);
        }

        private ObservableCollection<string> modelCategories;
        public ObservableCollection<string> ModelCategories {
            get => modelCategories;
            protected set => SetProperty(ref modelCategories, value);
        }

        private ICommand addModelCommand;
        public ICommand AddModelCommand => addModelCommand ??= new DelegateCommand<string>(CreateModel);

        private ICommand removeModelCommand;
        public ICommand RemoveModelCommand => removeModelCommand ??= new DelegateCommand<TModel>(RemoveModel);

        private ICommand addCategoryCommand;
        public ICommand AddCategoryCommand => addCategoryCommand ??= new DelegateCommand(CreateCategory);

        private ICommand removeCategoryCommand;
        public ICommand RemoveCategoryCommand => removeCategoryCommand ??= new DelegateCommand<string>(RemoveCategory);

        private ICommand showCategoryCommand;
        public ICommand ShowCategoryCommand => showCategoryCommand ??= new DelegateCommand<string>(ShowCategory);

        private ICommand openEditorCommand;
        public ICommand OpenEditorCommand => openEditorCommand ??= new DelegateCommand<TModel>(OpenEditor);

        protected abstract string RelativePath { get; }

        protected virtual string GetRelativeFilePath(TModel model) => RelativePath + "/" + model.Id + ".json";

        public override async Task OnNavigatedToAsync(NavigationContext navigationContext)
        {
            Models = new ObservableCollection<TModel>();
            ModelCategories = new ObservableCollection<string>();
            string[] jsons = new string[0];
            try
            {
                jsons = await Session.LoadJsonsAsync(RelativePath);
            }
            catch (Exception ex)
            {
                Context.SnackbarService.Enqueue("Failed to load jsons, you can try again by refreshing tab");
            }
            foreach (string json in jsons)
            {
                try
                {
                    TModel model = JsonConvert.DeserializeObject<TModel>(json);
                    Models.Add(model);
                }
                catch (Exception ex)
                {
                    Context.SnackbarService.Enqueue("Found invalid json");
                }
            }
            ModelCategories.AddRange(Models.Select(x => x.Category).Distinct());
        }

        protected virtual async void OpenEditor(TModel model)
        {
            TModel copiedModel = (TModel)model.DeepClone();
            bool save = await Context.DialogService.ShowModelDialogAsync(copiedModel);
            if (save)
            {
                model.CopyValues(copiedModel);
                string json = JsonConvert.SerializeObject(model);
                bool saved = await Context.Session.SaveJsonFileAsync(GetRelativeFilePath(model), json);
                Context.SnackbarService.Enqueue(saved ? "Saved successfully" : "Couldn't save model");
            }
        }

        protected virtual void ShowCategory(string category)
        {
            CurrentCategoryModels = new ObservableCollection<TModel>();
            CurrentCategoryModels.AddRange(Models.Where(x => string.Compare(x.Category, category) == 0));
        }

        protected virtual TModel CreateModelInstance() => Activator.CreateInstance<TModel>();

        protected virtual async void CreateModel(string category)
        {
            TModel newModel = CreateModelInstance();
            int nextId = Models.Count > 0 ? Models.Max(x => x.Id) + 1 : 0;
            newModel.Id = nextId;
            newModel.Title = "New model";
            newModel.Category = category;
            string json = JsonConvert.SerializeObject(newModel);
            string relativeFilePath = GetRelativeFilePath(newModel);
            bool created = await Session.SaveJsonFileAsync(relativeFilePath, json);
            if (created)
            {
                Models.Add(newModel);
                CurrentCategoryModels.Add(newModel);
            }
        }

        protected virtual async void RemoveModel(TModel model)
        {
            bool removed = Models.Remove(model);
            if (removed)
            {
                string relativeFilePath = GetRelativeFilePath(model);
                bool deleted = await Session.DeleteFileAsync(relativeFilePath);
                if (deleted)
                {
                    CurrentCategoryModels.Remove(model);
                }
                else
                {
                    Models.Add(model);
                }
            }
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
