using Microsoft.Extensions.Logging;
using ResourceManager;
using ResourceManager.Data;
using ResourceManager.DataSource;
using RPGDataEditor.Mvvm.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace RPGDataEditor.Mvvm
{
    public abstract class PresentableDataViewModel<TResource> : ModelsManagerViewModel<PresentableData> where TResource : IIdentifiable
    {
        public PresentableDataViewModel(IViewService viewService, IDataSource dataSource, ILogger<PresentableDataViewModel<TResource>> logger)
            : base(viewService, dataSource, logger) { }

        protected abstract TResource CreateResource(PresentableData model);

        public override Task Refresh()
        {
            IsLoading = true;
            Models = new ObservableCollection<PresentableData>();
            try
            {
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
            return Task.CompletedTask;
        }

        protected virtual PresentableData CreatePresentableData(string location)
        {
            PresentableData presentable = CreateModelInstance();
            LocationResourceDescriptor pathDescriptor = DataSource.DescriptorService.GetRequiredDescriptor<LocationResourceDescriptor>(typeof(TResource));
            KeyValuePair<string, object>[] parameters = pathDescriptor.ParseParameters(location);
            presentable.Id = parameters.FirstOrDefault(x => string.Compare(x.Key, nameof(PresentableData.Id), true) == 0).Value;
            presentable.Name = parameters.FirstOrDefault(x => string.Compare(x.Key, nameof(PresentableData.Name), true) == 0).Value?.ToString();
            return presentable;
        }

        protected virtual Task<TResource> RetrieveResource(PresentableData model)
        {
            TResource resource;
            try
            {
                resource = DataSource.Query<TResource>().First(x => IdentityEqualityComparer.Default.Equals(x.Id, model.Id));
            }
            catch (Exception ex)
            {
                resource = CreateResource(model);
                if (resource is null)
                {
                    Logger.LogError(ex, "Couldn't retrieve model " + typeof(TResource));
                }
            }
            return Task.FromResult(resource);
        }

        protected override async Task<EditorResults> OpenEditorAsync(PresentableData model)
        {
            TResource oldResource = await RetrieveResource(model);
            if (oldResource == null)
            {
                return new EditorResults(null, false);
            }
            TResource newResource = (TResource)oldResource.DeepClone();
            bool result = await ViewService.DialogService.ShowModelDialogAsync(newResource);
            EditorResults results = new EditorResults(newResource, result);
            if (results.Success)
            {
                try
                {
                    TrackedResource<TResource> tracked = await DataSource.UpdateAsync(newResource);
                    await DataSource.SaveChangesAsync();
                    model.Update(newResource);
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, $"Couldn't update model of type {typeof(TResource)}");
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
            bool result = await ViewService.DialogService.ShowModelDialogAsync(newResource);
            if (result)
            {
                try
                {
                    TrackedResource<TResource> tracked = await DataSource.AddAsync(newResource);
                    await DataSource.SaveChangesAsync();
                    Models.Add(newPresentable);
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, $"Couldn't add model of type {typeof(TResource)}");
                }
            }
            return newPresentable;
        }

        protected virtual object GetNextId() => Models.Count > 0 ? Models.Select(x => x.Id).Cast<int>().Max() + 1 : (object)1;
    }
}
