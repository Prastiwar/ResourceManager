using ResourceManager;
using ResourceManager.Commands;
using RPGDataEditor.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace RPGDataEditor.Mvvm
{
    public abstract class PresentableDataViewModel<TResource> : ModelsManagerViewModel<PresentableData> where TResource : IIdentifiable
    {
        public PresentableDataViewModel(ViewModelContext context) : base(context) { }

        public override async Task Refresh()
        {
            IsLoading = true;
            Models = new ObservableCollection<PresentableData>();
            try
            {
                IEnumerable<PresentableData> models = await Context.Mediator.Send(new GetPresentableResourceByIdQuery<IEnumerable<PresentableData>>());
                Models.AddRange(models);
            }
            catch (Exception ex)
            {
                Context.Logger.Error("Failed to get resources at " + GetType().Name, ex);
            }
            IsLoading = false;
        }

        protected virtual async Task<TResource> RetrieveResource(PresentableData model)
        {
            TResource resource;
            try
            {
                resource = await Context.Mediator.Send(new GetResourceByIdQuery<TResource>(model.Id));
            }
            catch (Exception ex)
            {
                resource = CreateResource(model);
                if (resource is null)
                {
                    Context.Logger.Error("Couldn't retrieve model " + typeof(TResource), ex);
                }
            }
            return resource;
        }

        protected virtual TResource CreateResource(PresentableData model) => default;

        protected override async Task<EditorResults> OpenEditorAsync(PresentableData model)
        {
            TResource oldModel = await RetrieveResource(model);
            if (oldModel == null)
            {
                return new EditorResults(null, false);
            }
            TResource newModel = (TResource)oldModel.DeepClone();
            bool saveRequested = await Context.DialogService.ShowModelDialogAsync(newModel);
            EditorResults results = new EditorResults(newModel, saveRequested);
            if (saveRequested)
            {
                UpdateResourceResults updateResults = await Context.Mediator.Send(new UpdateResourceQuery<TResource>(oldModel, newModel));
            }
            return results;
        }

        protected override PresentableData CreateModelInstance() => new PresentableData(typeof(TResource));

        protected override async Task<PresentableData> CreateModelAsync()
        {
            PresentableData newPresentable = CreateModelInstance();
            newPresentable.Id = GetNextId();
            TResource newResource = CreateResource(newPresentable);
            bool saveRequested = await Context.DialogService.ShowModelDialogAsync(newResource);
            if (saveRequested)
            {
                CreateResourceResults createResults = await Context.Mediator.Send(new CreateResourceQuery<TResource>(newResource));
                if (createResults.IsSuccess)
                {
                    Models.Add(newPresentable);
                }
            }
            return newPresentable;
        }

        protected virtual object GetNextId() => Models.Count > 0 ? Models.Select(x => x.Id).Cast<int>().Max() + 1 : (object)1;
    }
}
