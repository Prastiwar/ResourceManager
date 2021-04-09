using Prism.Commands;
using Prism.Regions;
using RPGDataEditor.Core.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RPGDataEditor.Core.Mvvm
{
    public abstract class IdentifiableTabViewModel<TModel> : TabViewModel where TModel : ObservableModel, IIdentifiable
    {
        public IdentifiableTabViewModel(ViewModelContext context, ITypeToResourceConverter resourceConverter) : base(context)
            => ResourceConverter = resourceConverter;

        private ObservableCollection<TModel> models;
        public ObservableCollection<TModel> Models {
            get => models;
            protected set => SetProperty(ref models, value);
        }

        private ICommand addModelCommand;
        public ICommand AddModelCommand => addModelCommand ??= new DelegateCommand(CreateModel);

        private ICommand removeModelCommand;
        public ICommand RemoveModelCommand => removeModelCommand ??= new DelegateCommand<TModel>(RemoveModel);

        private ICommand openEditorCommand;
        public ICommand OpenEditorCommand => openEditorCommand ??= new DelegateCommand<TModel>(OpenEditor);

        protected ITypeToResourceConverter ResourceConverter { get; }

        public override async Task Refresh()
        {
            IsLoading = true;
            Models = new ObservableCollection<TModel>();
            try
            {
                IIdentifiable[] foundModels = await Session.Client.GetAllAsync(ResourceConverter.ToResource(typeof(TModel)));
                Models.AddRange(foundModels.Select(resource => resource as TModel));
            }
            catch (Exception ex)
            {
                Logger.Error("Failed to load jsons, class: " + GetType().Name, ex);
                Context.SnackbarService.Enqueue("Failed to load jsons, you can try again by refreshing tab");
            }
            IsLoading = false;
        }

        public override Task OnNavigatedToAsync(NavigationContext navigationContext) => Refresh();

        private async void OpenEditor(TModel model) => await OpenEditorAsync(model);

        protected virtual async Task<EditorResults> OpenEditorAsync(TModel model)
        {
            TModel newModel = (TModel)model.DeepClone();
            bool save = await Context.DialogService.ShowModelDialogAsync(newModel);
            if (save)
            {
                bool saved = await Context.Session.Client.UpdateAsync(model, newModel);
                if (saved)
                {
                    model.CopyValues(newModel);
                }
                Context.SnackbarService.Enqueue(saved ? "Saved successfully" : "Couldn't save model");
            }
            return new EditorResults(newModel, save);
        }

        protected virtual TModel CreateModelInstance() => Activator.CreateInstance<TModel>();

        private async void CreateModel() => await CreateModelAsync();

        protected virtual async Task<TModel> CreateModelAsync()
        {
            TModel newModel = CreateModelInstance();
            int nextId = Models.Count > 0 ? Models.Max(x => (int)x.Id) + 1 : 0;
            newModel.Id = nextId;

            bool save = await Context.DialogService.ShowModelDialogAsync(newModel);
            if (save)
            {
                bool saved = await Context.Session.Client.CreateAsync(newModel);
                if (saved)
                {
                    Models.Add(newModel);
                }
                Context.SnackbarService.Enqueue(saved ? "Created successfully" : "Couldn't create model");
            }
            return newModel;
        }

        private async void RemoveModel(TModel model) => await RemoveModelAsync(model);

        protected virtual async Task<bool> RemoveModelAsync(TModel model)
        {
            bool removed = Models.Remove(model);
            if (removed)
            {
                bool deleted = await Session.Client.DeleteAsync(model);
                if (!deleted)
                {
                    Models.Add(model);
                }
                return deleted;
            }
            return removed;
        }
    }
}
