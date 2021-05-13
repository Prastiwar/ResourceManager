using ResourceManager;
using ResourceManager.Commands;
using RPGDataEditor.Commands;
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
                IEnumerable<PresentableData> models = await Context.Mediator.Send(new GetPresentablesByIdQuery(typeof(TResource), null));
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
                resource = (TResource)await Context.Mediator.Send(new GetResourceByIdQuery(typeof(TResource), model.Id));
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
            TResource oldResource = await RetrieveResource(model);
            if (oldResource == null)
            {
                return new EditorResults(null, false);
            }
            TResource newResource = (TResource)oldResource.DeepClone();
            bool saveRequested = await Context.DialogService.ShowModelDialogAsync(newResource);
            EditorResults results = new EditorResults(newResource, saveRequested);
            if (results.Success)
            {
                UpdateResourceResults updateResults = await Context.Mediator.Send(new UpdateResourceQuery(typeof(TResource), oldResource, newResource));
                results.Success = updateResults.IsSuccess;
                if (results.Success)
                {
                    model.Update(newResource);
                }
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
                CreateResourceResults createResults = await Context.Mediator.Send(new CreateResourceQuery(typeof(TResource), newResource));
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
