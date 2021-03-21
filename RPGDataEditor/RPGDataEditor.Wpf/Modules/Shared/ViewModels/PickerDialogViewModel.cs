using Prism.Services.Dialogs;
using RPGDataEditor.Core;
using RPGDataEditor.Core.Models;
using RPGDataEditor.Core.Mvvm;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RPGDataEditor.Wpf.ViewModels
{
    public class PickerDialogViewModel : DialogViewModelBase
    {
        public PickerDialogViewModel(ViewModelContext context, ILocationToSimpleResourceConverter simpleResourceConverter) : base(context) => this.simpleResourceConverter = simpleResourceConverter;

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

        private readonly ILocationToSimpleResourceConverter simpleResourceConverter;

        protected sealed override void CloseDialog(object result) => Close(result is bool b && b);

        protected virtual async void Close(bool result)
        {
            bool isCancelled = await ShouldCancelAsync(result).ConfigureAwait(true);
            if (!isCancelled)
            {
                await OnDialogClosing(result).ConfigureAwait(true);
                Close(new PickerDialogParameters(Model is NullResource ? null : Model).WithResult(result).Build());
            }
        }

        public virtual Task OnDialogClosing(bool result) => Task.FromResult(true);

        protected virtual Task<bool> ShouldCancelAsync(bool result) => Task.FromResult(false);

        protected override async Task InitializeAsync(IDialogParameters parameters)
        {
            IsLoading = true;
            RPGResource resource = parameters.GetValue<RPGResource>(nameof(PickerDialogParameters.PickResource));
            List<IIdentifiable> list = await LoadResourcesAsync(resource);
            list.Sort(new IdentifiableComparer());
            Models = list;

            IIdentifiable model = parameters.GetValue<IIdentifiable>(nameof(PickerDialogParameters.PickedItem));
            int modelId = parameters.GetValue<int>(nameof(PickerDialogParameters.PickedId));
            if (model == null)
            {
                model = Models.FirstOrDefault(x => (int)x.Id == modelId);
            }
            else
            {
                model = Models.FirstOrDefault(x => x.Id == model.Id);
            }
            Model = model ?? Models.First();
            IsLoading = false;
        }

        protected virtual async Task<List<IIdentifiable>> LoadResourcesAsync(RPGResource resource)
        {
            string[] locations = await Session.Client.GetAllLocationsAsync((int)resource);
            List<IIdentifiable> list = new List<IIdentifiable>(locations.Length + 1) {
                new NullResource()
            };
            foreach (string location in locations)
            {
                list.Add(simpleResourceConverter.CreateSimpleData(location));
            }
            return list;
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
