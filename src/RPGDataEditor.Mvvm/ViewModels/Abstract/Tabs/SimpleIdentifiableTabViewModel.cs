using RPGDataEditor.Core;
using RPGDataEditor.Mvvm.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace RPGDataEditor.Mvvm
{
    public abstract partial class SimpleIdentifiableTabViewModel<TModel> : ModelsManagerViewModel<SimpleIdentifiableData> where TModel : ObservableModel, IIdentifiable
    {
        public SimpleIdentifiableTabViewModel(ViewModelContext context) : base(context) { }

        public override async Task Refresh()
        {
            IsLoading = true;
            Models = new ObservableCollection<SimpleIdentifiableData>();
            try
            {
                string[] files = await Context.ConnectionService.Client.GetAllLocationsAsync(ResourceConverter.ToResource(typeof(TModel)));
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
            TModel oldModel = await RetrieveModel(model);
            if (oldModel == null)
            {
                Context.SnackbarService.Enqueue("Couldn't open editor for this model");
                return new EditorResults(null, false);
            }

            TModel newModel = oldModel.DeepClone() as TModel;
            return await CreateOrEditAsync(model, oldModel, newModel);
        }

        protected virtual async Task<EditorResults> CreateOrEditAsync(SimpleIdentifiableData model, TModel oldModel, TModel newModel)
        {
            bool saveRequested = await Context.DialogService.ShowModelDialogAsync(newModel);
            EditorResults results = new EditorResults(newModel, saveRequested);
            if (saveRequested)
            {
                await OnSavingAsync(model, results);
                if (!results.Success)
                {
                    return results;
                }
                bool saved = false;
                if (oldModel != null)
                {
                    saved = await Context.ConnectionService.Client.UpdateAsync(oldModel, newModel);
                }
                else
                {
                    saved = await Context.ConnectionService.Client.CreateAsync(newModel);
                }
                Context.SnackbarService.Enqueue(saved ? "Saved successfully" : "Couldn't save model");
            }
            return results;
        }

        protected virtual TModel CreateNewExactModel(SimpleIdentifiableData model) => null;

        protected virtual Task OnSavingAsync(SimpleIdentifiableData model, EditorResults results) => Task.CompletedTask;

        protected virtual async Task<TModel> RetrieveModel(SimpleIdentifiableData model)
        {
            TModel actualModel = null;
            try
            {
                actualModel = (TModel)await Context.ConnectionService.Client.GetAsync(model);
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

        protected virtual SimpleIdentifiableData CreateSimpleModel(string file) => simpleResourceConverter.CreateSimpleData(file);

        protected override SimpleIdentifiableData CreateModelInstance() => new SimpleIdentifiableData(typeof(TModel));

        protected override async Task<SimpleIdentifiableData> CreateModelAsync()
        {
            SimpleIdentifiableData newModel = CreateModelInstance();
            int nextId = Models.Count > 0 ? Models.Max(x => x.Id) + 1 : 0;
            newModel.Id = nextId;
            TModel newExactModel = CreateNewExactModel(newModel);
            EditorResults results = await CreateOrEditAsync(newModel, null, newExactModel);
            if (results.Success)
            {
                Models.Add(newModel);
            }
            return newModel;
        }
    }
}
