using Newtonsoft.Json;
using RPGDataEditor.Core.Models;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RPGDataEditor.Core.Mvvm
{
    public abstract partial class SimpleIdentifiableTabViewModel<TModel> : IdentifiableTabViewModel<SimpleIdentifiableData> where TModel : ObservableModel, IIdentifiable
    {
        public SimpleIdentifiableTabViewModel(ViewModelContext context) : base(context) { }

        protected override string GetRelativeFilePath(SimpleIdentifiableData model) => RelativePath + $"/{model.Id}_{model.Name}.json";

        public override async Task Refresh()
        {
            IsLoading = true;
            Models = new ObservableCollection<SimpleIdentifiableData>();
            try
            {
                string[] files = await Session.GetFilesAsync(RelativePath);
                foreach (string file in files)
                {
                    SimpleIdentifiableData newModel = CreateSimpleModel(file);
                    Models.Add(newModel);
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Failed to load files, class: " + GetType().Name, ex);
                Context.SnackbarService.Enqueue("Failed to load files, you can try again by refreshing tab");
            }
            IsLoading = false;
        }

        protected override async Task<EditorResults> OpenEditorAsync(SimpleIdentifiableData model)
        {
            string filePath = GetRelativeFilePath(model);
            TModel actualModel = await RetrieveModel(model);
            if (actualModel == null)
            {
                return new EditorResults(null, false);
            }

            bool saveRequested = await Context.DialogService.ShowModelDialogAsync(actualModel);
            EditorResults results = new EditorResults(actualModel, saveRequested);
            if (saveRequested)
            {
                await OnSavingAsync(model, results);
                if (!results.Success)
                {
                    return results;
                }
                string newFilePath = GetRelativeFilePath(model);
                string saveJson = JsonConvert.SerializeObject(actualModel);
                bool saved = await Context.Session.SaveJsonFileAsync(newFilePath, saveJson);
                if (saved && filePath.CompareTo(newFilePath) != 0)
                {
                    bool deleted = await Context.Session.DeleteFileAsync(filePath);
                    if (!deleted)
                    {
                        Context.SnackbarService.Enqueue("Couldn't delete old model");
                    }
                }
                Context.SnackbarService.Enqueue(saved ? "Saved successfully" : "Couldn't save model");
            }
            return results;
        }

        protected virtual TModel CreateNewExactModel(SimpleIdentifiableData model) => null;

        protected virtual Task OnSavingAsync(SimpleIdentifiableData model, EditorResults results) => Task.CompletedTask;

        protected virtual async Task<TModel> RetrieveModel(SimpleIdentifiableData model)
        {
            string filePath = GetRelativeFilePath(model);
            TModel actualModel = null;
            try
            {
                string readJson = await Session.GetJsonAsync(filePath);
                actualModel = JsonConvert.DeserializeObject<TModel>(readJson);
            }
            catch (Exception ex)
            {
                actualModel = CreateNewExactModel(model);
                if (actualModel == null)
                {
                    Logger.Error("Couldn't retrieve model " + typeof(TModel), ex);
                }
            }
            return actualModel;
        }

        protected virtual SimpleIdentifiableData CreateSimpleModel(string file)
        {
            string fileName = Path.GetFileNameWithoutExtension(file);
            int id = -1;
            string name = fileName;
            int index = fileName.IndexOf('_');
            if (index >= 0)
            {
                string idString = fileName.Substring(0, index);
                if (int.TryParse(idString, out int newId))
                {
                    id = newId;
                }
                name = fileName[(index + 1)..];
            }
            return new SimpleIdentifiableData() {
                Id = id,
                Name = name
            };
        }

        protected override Task<SimpleIdentifiableData> CreateModelAsync()
        {
            SimpleIdentifiableData newModel = CreateModelInstance();
            int nextId = Models.Count > 0 ? Models.Max(x => x.Id) + 1 : 0;
            newModel.Name = "New Model";
            newModel.Id = nextId;
            Models.Add(newModel);
            return Task.FromResult(newModel);
        }
    }
}
