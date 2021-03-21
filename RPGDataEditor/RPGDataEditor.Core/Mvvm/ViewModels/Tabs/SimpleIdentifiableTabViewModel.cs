using RPGDataEditor.Core.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace RPGDataEditor.Core.Mvvm
{
    public abstract partial class SimpleIdentifiableTabViewModel<TModel> : IdentifiableTabViewModel<SimpleIdentifiableData> where TModel : ObservableModel, IIdentifiable
    {

        public SimpleIdentifiableTabViewModel(ViewModelContext context,
                                              ITypeToResourceConverter resourceConverter,
                                              ILocationToSimpleResourceConverter simpleResourceConverter)
            : base(context, resourceConverter) => this.simpleResourceConverter = simpleResourceConverter;

        private readonly ILocationToSimpleResourceConverter simpleResourceConverter;

        public override async Task Refresh()
        {
            IsLoading = true;
            Models = new ObservableCollection<SimpleIdentifiableData>();
            try
            {
                string[] files = await Session.Client.GetAllLocationsAsync(ResourceConverter.ToResource(typeof(TModel)));
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

            TModel newModel = oldModel.DeepClone<TModel>();
            bool saveRequested = await Context.DialogService.ShowModelDialogAsync(newModel);
            EditorResults results = new EditorResults(newModel, saveRequested);
            if (saveRequested)
            {
                await OnSavingAsync(model, results);
                if (!results.Success)
                {
                    return results;
                }
                bool saved = await Context.Session.Client.UpdateAsync(oldModel, newModel);
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
                actualModel = (TModel)await Session.Client.GetAsync(model);
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
