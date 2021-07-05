using Microsoft.Extensions.Logging;
using ResourceManager.Data;
using ResourceManager.DataSource;
using ResourceManager.Mvvm.Navigation;
using ResourceManager.Mvvm.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace ResourceManager.Mvvm
{
    public abstract class ModelsManagerViewModel<TModel> : ScreenViewModel where TModel : IIdentifiable
    {
        public ModelsManagerViewModel(IViewService viewService, IDataSource dataSource, ILogger<ModelsManagerViewModel<TModel>> logger) : base()
        {
            ViewService = viewService;
            DataSource = dataSource;
            Logger = logger;
        }

        private ObservableCollection<TModel> models;
        public ObservableCollection<TModel> Models {
            get => models;
            protected set => SetProperty(ref models, value);
        }

        protected IViewService ViewService { get; }

        protected IDataSource DataSource { get; }

        protected ILogger<ModelsManagerViewModel<TModel>> Logger { get; }

        public override Task OnNavigatedToAsync(INavigationContext navigationContext) => Refresh();

        public override Task Refresh()
        {
            IsLoading = true;
            Models = new ObservableCollection<TModel>();
            try
            {
                System.Collections.Generic.List<TModel> models = DataSource.Query<TModel>().ToList();
                Models.AddRange(models);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed to get resources at " + GetType().Name);
            }
            IsLoading = false;
            return Task.CompletedTask;
        }

        protected virtual TModel CreateModelInstance() => Activator.CreateInstance<TModel>();

        protected virtual async Task<TModel> CreateModelAsync()
        {
            TModel newModel = CreateModelInstance();

            int nextId = Models.Count > 0 ? Models.Max(x => (int)x.Id) + 1 : 0;
            newModel.Id = nextId;

            bool result = await ViewService.DialogService.ShowModelDialogAsync(newModel);
            if (result)
            {
                try
                {
                    newModel.Id = null;
                    TrackedResource<TModel> tracked = await DataSource.AddAsync(newModel);
                    await DataSource.SaveChangesAsync();
                    Models.Add(tracked.Resource);
                    return tracked.Resource;
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, $"Couldn't add model of type {typeof(TModel)}");
                }
            }
            return default;
        }

        protected virtual async Task<bool> RemoveModelAsync(TModel model)
        {
            bool removed = Models.Remove(model);
            if (removed)
            {
                try
                {
                    TrackedResource<TModel> tracked = await DataSource.DeleteAsync(model);
                    await DataSource.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    Models.Add(model);
                    Logger.LogError(ex, $"Couldn't remove model of type {typeof(TModel)}");
                }
            }
            return removed;
        }

        protected virtual async Task<EditorResults> OpenEditorAsync(TModel model)
        {
            TModel newModel = (TModel)model.DeepClone();
            bool result = await ViewService.DialogService.ShowModelDialogAsync(newModel);
            if (result)
            {
                try
                {
                    TrackedResource<TModel> tracked = await DataSource.UpdateAsync(model);
                    await DataSource.SaveChangesAsync();
                    model.CopyProperties(newModel);
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, $"Couldn't update model of type {typeof(TModel)}");
                }
            }
            return new EditorResults(newModel, result);
        }
    }
}
