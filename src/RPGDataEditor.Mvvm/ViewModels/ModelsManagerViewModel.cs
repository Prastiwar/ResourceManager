using MediatR;
using Microsoft.Extensions.Logging;
using ResourceManager;
using ResourceManager.Commands;
using ResourceManager.Data;
using ResourceManager.DataSource;
using RPGDataEditor.Mvvm.Commands;
using RPGDataEditor.Mvvm.Navigation;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace RPGDataEditor.Mvvm
{
    public abstract class ModelsManagerViewModel<TModel> : ScreenViewModel where TModel : IIdentifiable
    {
        public ModelsManagerViewModel(IDataSource dataSource, ILogger<ModelsManagerViewModel<TModel>> logger) : base()
        {
            DataSource = dataSource;
            Logger = logger;
        }

        private ObservableCollection<TModel> models;
        public ObservableCollection<TModel> Models {
            get => models;
            protected set => SetProperty(ref models, value);
        }

        protected IDataSource DataSource { get; }
        protected ILogger<ModelsManagerViewModel<TModel>> Logger { get; }

        public override Task OnNavigatedToAsync(INavigationContext navigationContext) => Refresh();

        public override async Task Refresh()
        {
            IsLoading = true;
            Models = new ObservableCollection<TModel>();
            try
            {
                //IEnumerable<TModel> models = (await Mediator.Send(new GetResourcesByIdQuery(typeof(TModel), null))).Cast<TModel>();
                var models = DataSource.Query<TModel>().ToList();
                Models.AddRange(models);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed to get resources at " + GetType().Name);
            }
            IsLoading = false;
        }

        protected virtual TModel CreateModelInstance() => Activator.CreateInstance<TModel>();

        protected virtual async Task<TModel> CreateModelAsync()
        {
            TModel newModel = CreateModelInstance();

            int nextId = Models.Count > 0 ? Models.Max(x => (int)x.Id) + 1 : 0;
            newModel.Id = nextId;

            IDialogResult result = await Mediator.Send(ShowDialogQueryHelper.CreateModelQuery(newModel));
            if (result.IsSuccess)
            {
                CreateResourceResults results = await Mediator.Send(new CreateResourceRequest(typeof(TModel), newModel));
                bool created = results.IsSuccess;
                if (created)
                {
                    Models.Add(newModel);
                }
            }
            return newModel;
        }

        protected virtual async Task<bool> RemoveModelAsync(TModel model)
        {
            bool removed = Models.Remove(model);
            if (removed)
            {
                DeleteResourceResults results = await Mediator.Send(new DeleteResourceRequest(typeof(TModel), model));
                bool deleted = results.IsSuccess;
                if (!deleted)
                {
                    Models.Add(model);
                }
                return deleted;
            }
            return removed;
        }

        protected virtual async Task<EditorResults> OpenEditorAsync(TModel model)
        {
            TModel newModel = (TModel)model.DeepClone();
            IDialogResult result = await Mediator.Send(ShowDialogQueryHelper.CreateModelQuery(newModel));
            if (result.IsSuccess)
            {
                UpdateResourceResults results = await Mediator.Send(new UpdateResourceQuery(typeof(TModel), model, newModel));
                bool updated = results.IsSuccess;
                if (updated)
                {
                    model.CopyProperties(newModel);
                }
            }
            return new EditorResults(newModel, result.IsSuccess);
        }
    }
}
