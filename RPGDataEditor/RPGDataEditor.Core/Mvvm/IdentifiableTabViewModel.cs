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
    public abstract class IdentifiableTabViewModel<TModel> : TabViewModel where TModel : ObservableModel, IIdentifiable
    {
        public IdentifiableTabViewModel(ViewModelContext context) : base(context) { }

        private ObservableCollection<TModel> models;
        public ObservableCollection<TModel> Models {
            get => models;
            protected set => SetProperty(ref models, value);
        }

        private ICommand addModelCommand;
        public ICommand AddModelCommand => addModelCommand ??= new DelegateCommand<string>(CreateModel);

        private ICommand removeModelCommand;
        public ICommand RemoveModelCommand => removeModelCommand ??= new DelegateCommand<TModel>(RemoveModel);

        private ICommand openEditorCommand;
        public ICommand OpenEditorCommand => openEditorCommand ??= new DelegateCommand<TModel>(OpenEditor);

        protected abstract string RelativePath { get; }

        protected virtual string GetRelativeFilePath(TModel model) => RelativePath + "/" + model.Id + ".json";

        public override async Task Refresh()
        {
            Models = new ObservableCollection<TModel>();
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
        }

        public override Task OnNavigatedToAsync(NavigationContext navigationContext) => Refresh();

        private async void OpenEditor(TModel model) => await OpenEditorAsync(model);

        protected virtual async Task OpenEditorAsync(TModel model)
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

        protected virtual TModel CreateModelInstance() => Activator.CreateInstance<TModel>();

        private async void CreateModel(string category) => await CreateModelAsync(category);

        protected virtual async Task<TModel> CreateModelAsync(string category)
        {
            TModel newModel = CreateModelInstance();
            int nextId = Models.Count > 0 ? Models.Max(x => x.Id) + 1 : 0;
            newModel.Id = nextId;
            string json = JsonConvert.SerializeObject(newModel);
            string relativeFilePath = GetRelativeFilePath(newModel);
            bool created = await Session.SaveJsonFileAsync(relativeFilePath, json);
            if (created)
            {
                Models.Add(newModel);
                return newModel;
            }
            return null;
        }

        private async void RemoveModel(TModel model) => await RemoveModelAsync(model);

        protected virtual async Task<bool> RemoveModelAsync(TModel model)
        {
            bool removed = Models.Remove(model);
            if (removed)
            {
                string relativeFilePath = GetRelativeFilePath(model);
                bool deleted = await Session.DeleteFileAsync(relativeFilePath);
                if (!deleted)
                {
                    Models.Add(model);
                }
                return deleted;
            }
            return removed;
        }
    }
}
