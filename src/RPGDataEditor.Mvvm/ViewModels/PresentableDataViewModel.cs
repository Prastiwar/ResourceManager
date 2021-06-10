using Microsoft.Extensions.Logging;
using ResourceManager;
using ResourceManager.Commands;
using ResourceManager.Data;
using ResourceManager.DataSource;
using ResourceManager.Services;
using RPGDataEditor.Mvvm.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace RPGDataEditor.Mvvm
{
    public abstract class PresentableDataViewModel<TResource> : ModelsManagerViewModel<PresentableData> where TResource : IIdentifiable
    {
        public PresentableDataViewModel(IResourceDescriptorService descriptorService, IDataSource dataSource, ILogger<PresentableDataViewModel<TResource>> logger)
            : base(dataSource, logger) => DescriptorService = descriptorService;

        protected IResourceDescriptorService DescriptorService { get; }

        protected abstract TResource CreateResource(PresentableData model);

        public override async Task Refresh()
        {
            IsLoading = true;
            Models = new ObservableCollection<PresentableData>();
            try
            {
                //IEnumerable<PresentableData> models = await Mediator.Send(new GetPresentablesByIdQuery(typeof(TResource), null));
                string[] modelLocations = DataSource.Locate(typeof(TResource)).ToArray();
                foreach (string location in modelLocations)
                {
                    PresentableData data = CreatePresentableData(location);
                    Models.Add(data);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed to get resources at " + GetType().Name);
            }
            IsLoading = false;
        }

        protected virtual PresentableData CreatePresentableData(string location)
        {
            PresentableData presentable = CreateModelInstance();
            PathResourceDescriptor pathDescriptor = DescriptorService.GetRequiredDescriptor<PathResourceDescriptor>(typeof(TResource));
            KeyValuePair<string, object>[] parameters = pathDescriptor.ParseParameters(location);
            presentable.Id = parameters.FirstOrDefault(x => string.Compare(x.Key, nameof(PresentableData.Id), true) == 0).Value;
            presentable.Name = parameters.FirstOrDefault(x => string.Compare(x.Key, nameof(PresentableData.Name), true) == 0).Value?.ToString();
            return presentable;
        }

        protected virtual async Task<TResource> RetrieveResource(PresentableData model)
        {
            TResource resource;
            try
            {
                resource = (TResource)await Mediator.Send(new GetResourceByIdQuery(typeof(TResource), model.Id));
            }
            catch (Exception ex)
            {
                resource = CreateResource(model);
                if (resource is null)
                {
                    Logger.LogError(ex, "Couldn't retrieve model " + typeof(TResource));
                }
            }
            return resource;
        }

        protected override async Task<EditorResults> OpenEditorAsync(PresentableData model)
        {
            TResource oldResource = await RetrieveResource(model);
            if (oldResource == null)
            {
                return new EditorResults(null, false);
            }
            TResource newResource = (TResource)oldResource.DeepClone();
            Navigation.IDialogResult result = await Mediator.Send(ShowDialogQueryHelper.CreateModelQuery(newResource));
            EditorResults results = new EditorResults(newResource, result.IsSuccess);
            if (results.Success)
            {
                UpdateResourceResults updateResults = await Mediator.Send(new UpdateResourceQuery(typeof(TResource), oldResource, newResource));
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
            Navigation.IDialogResult result = await Mediator.Send(ShowDialogQueryHelper.CreateModelQuery(newResource));
            if (result.IsSuccess)
            {
                CreateResourceResults createResults = await Mediator.Send(new CreateResourceRequest(typeof(TResource), newResource));
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
