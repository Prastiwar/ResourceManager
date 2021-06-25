using Microsoft.Extensions.Logging;
using Prism.Services.Dialogs;
using ResourceManager;
using ResourceManager.Data;
using ResourceManager.DataSource;
using RPGDataEditor.Extensions.Prism.Wpf;
using RPGDataEditor.Mvvm;
using RPGDataEditor.Wpf.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;

namespace RPGDataEditor.Wpf.ViewModels
{
    public class PickerDialogViewModel : DialogViewModelBase
    {
        public PickerDialogViewModel(IDataSource dataSource, ILogger<PickerDialogViewModel> logger) : base(logger) => DataSource = dataSource;

        public override string Title => "Resource Picker";

        private bool isLoading;
        public bool IsLoading {
            get => isLoading;
            set => SetProperty(ref isLoading, value);
        }

        private IIdentifiable model;
        public IIdentifiable Model {
            get => model;
            set => SetProperty(ref model, value);
        }

        private IList<IIdentifiable> models;
        public IList<IIdentifiable> Models {
            get => models;
            set => SetProperty(ref models, value);
        }

        private IValueConverter modelNameConverter;
        public IValueConverter ModelNameConverter {
            get => modelNameConverter;
            set => SetProperty(ref modelNameConverter, value);
        }

        protected IDataSource DataSource { get; }

        protected sealed override void CloseDialog(object result) => Close(result is bool b && b);

        protected virtual async void Close(bool result)
        {
            bool isCancelled = await ShouldCancelAsync(result).ConfigureAwait(true);
            if (!isCancelled)
            {
                await OnDialogClosing(result).ConfigureAwait(true);
                Close(new PickerDialogParameters(Model is NullResource ? null : Model).WithResult(result).BuildPrism());
            }
        }

        public virtual Task OnDialogClosing(bool result) => Task.FromResult(true);

        protected virtual Task<bool> ShouldCancelAsync(bool result) => Task.FromResult(false);

        protected override async Task InitializeAsync(IDialogParameters parameters)
        {
            IsLoading = true;
            Type resource = parameters.GetValue<Type>(nameof(PickerDialogParameters.ResourceType));
            List<IIdentifiable> list = await LoadResourcesAsync(resource);
            list.Sort(new IdentifiableComparer());
            Models = list;

            ModelNameConverter = parameters.GetValue<IValueConverter>(nameof(PickerDialogParameters.ModelNameConverter));

            IIdentifiable model = parameters.GetValue<IIdentifiable>(nameof(PickerDialogParameters.PickedItem));
            int modelId = parameters.GetValue<int>(nameof(PickerDialogParameters.PickedId));
            if (model == null)
            {
                model = Models.FirstOrDefault(x => IdentityEqualityComparer.Default.Equals(x.Id, modelId));
            }
            else
            {
                model = Models.FirstOrDefault(x => IdentityEqualityComparer.Default.Equals(x.Id, model.Id));
            }
            Model = model ?? Models.First();
            IsLoading = false;
        }

        protected virtual Task<List<IIdentifiable>> LoadResourcesAsync(Type resourceType)
        {
            // TODO: Improve performance, .AsEnumerable() is not the best option
            List<IIdentifiable> resources = DataSource.Query(resourceType).AsEnumerable().Cast<IIdentifiable>().ToList();
            resources.Insert(0, new NullResource());
            return Task.FromResult(resources);
        }

        private class NullResource : IIdentifiable
        {
            public NullResource() => Id = -1;

            public object Id {
                get => -1;
                set { }
            }

            public override string ToString() => "None";
        }
    }
}
