using Newtonsoft.Json;
using RPGDataEditor.Core.Models;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;

namespace RPGDataEditor.Core.Mvvm
{
    public abstract class SimpleIdentifiableTabViewModel<TModel> : IdentifiableTabViewModel<SimpleIdentifiableData> where TModel : ObservableModel, IIdentifiable
    {
        public SimpleIdentifiableTabViewModel(ViewModelContext context) : base(context) { }

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

        protected override async Task OpenEditorAsync(SimpleIdentifiableData model)
        {
            string filePath = GetRelativeFilePath(model);
            string readJson = await Session.GetJsonAsync(filePath);
            TModel actualModel = JsonConvert.DeserializeObject<TModel>(readJson);
            bool save = await Context.DialogService.ShowModelDialogAsync(actualModel);
            if (save)
            {
                string saveJson = JsonConvert.SerializeObject(actualModel);
                bool saved = await Context.Session.SaveJsonFileAsync(filePath, saveJson);
                Context.SnackbarService.Enqueue(saved ? "Saved successfully" : "Couldn't save model");
            }
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

        protected sealed override SimpleIdentifiableData CreateModelInstance() => Activator.CreateInstance<SimpleIdentifiableData>();

        protected override Task<SimpleIdentifiableData> CreateModelAsync()
        {
            SimpleIdentifiableData newModel = CreateModelInstance();
            Models.Add(newModel);
            return Task.FromResult(newModel);
        }
    }
}
