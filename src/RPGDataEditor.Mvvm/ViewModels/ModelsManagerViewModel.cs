using ResourceManager;
using ResourceManager.Commands;
using RPGDataEditor.Mvvm.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace RPGDataEditor.Mvvm
{
    public abstract class ModelsManagerViewModel<TModel> : ScreenViewModel where TModel : IIdentifiable
    {
        public ModelsManagerViewModel(ViewModelContext context) : base(context) { }

        private ObservableCollection<TModel> models;
        public ObservableCollection<TModel> Models {
            get => models;
            protected set => SetProperty(ref models, value);
        }

        public override Task OnNavigatedToAsync(INavigationContext navigationContext) => Refresh();

        public override async Task Refresh()
        {
            IsLoading = true;
            Models = new ObservableCollection<TModel>();
            try
            {
                IEnumerable<TModel> models = (await Context.Mediator.Send(new GetResourcesByIdQuery(typeof(TModel), null))).Cast<TModel>();
                Models.AddRange(models);
            }
            catch (Exception ex)
            {
                Context.Logger.Error("Failed to get resources at " + GetType().Name, ex);
            }
            IsLoading = false;
        }

        protected virtual TModel CreateModelInstance() => Activator.CreateInstance<TModel>();

        protected virtual async Task<TModel> CreateModelAsync()
        {
            TModel newModel = CreateModelInstance();

            int nextId = Models.Count > 0 ? Models.Max(x => (int)x.Id) + 1 : 0;
            newModel.Id = nextId;

            bool create = await Context.DialogService.ShowModelDialogAsync(newModel);
            if (create)
            {
                CreateResourceResults results = await Context.Mediator.Send(new CreateResourceQuery<TModel>(newModel));
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
                DeleteResourceResults results = await Context.Mediator.Send(new DeleteResourceQuery<TModel>(model));
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
            bool update = await Context.DialogService.ShowModelDialogAsync(newModel);
            if (update)
            {
                UpdateResourceResults results = await Context.Mediator.Send(new UpdateResourceQuery<TModel>(model, newModel));
                bool updated = results.IsSuccess;
                if (updated)
                {
                    model.CopyProperties(newModel);
                }
            }
            return new EditorResults(newModel, update);
        }
    }
}
