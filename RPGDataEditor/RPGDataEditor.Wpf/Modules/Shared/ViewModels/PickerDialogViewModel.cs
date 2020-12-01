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
        public PickerDialogViewModel(ViewModelContext context) : base(context) { }

        public override string Title => "Resource Picker";

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

        protected sealed override void CloseDialog(object result) => Close(result is bool b && b);

        protected virtual async void Close(bool result)
        {
            bool isCancelled = await ShouldCancelAsync(result).ConfigureAwait(true);
            if (!isCancelled)
            {
                await OnDialogClosing(result).ConfigureAwait(true);
                Close(new PickerDialogParameters(Model is INullResource ? null : Model) { IsSuccess = result }.Build());
            }
        }

        public virtual Task OnDialogClosing(bool result) => Task.FromResult(true);

        protected virtual Task<bool> ShouldCancelAsync(bool result) => Task.FromResult(false);

        protected override async Task InitializeAsync(IDialogParameters parameters)
        {
            RPGResource resource = parameters.GetValue<RPGResource>(nameof(PickerDialogParameters.PickResource));
            List<IIdentifiable> list = await LoadResourcesAsync(resource);
            list.Sort(new IdentifiableComparer());
            Models = list;

            IIdentifiable model = parameters.GetValue<IIdentifiable>(nameof(PickerDialogParameters.PickedItem));
            int modelId = parameters.GetValue<int>(nameof(PickerDialogParameters.PickedId));
            if (model == null)
            {
                model = Models.FirstOrDefault(x => x.Id == modelId);
            }
            else
            {
                model = Models.FirstOrDefault(x => x.Id == model.Id);
            }
            Model = model ?? Models.First();
        }

        protected virtual async Task<List<IIdentifiable>> LoadResourcesAsync(RPGResource resource)
        {
            List<IIdentifiable> list = new List<IIdentifiable>();
            switch (resource)
            {
                case RPGResource.Quest:
                    list = new List<IIdentifiable>(await Context.Session.LoadAsync<QuestModel>("quests"));
                    list.Insert(0, new NullQuest());
                    break;
                case RPGResource.Dialogue:
                    list = new List<IIdentifiable>(await Context.Session.LoadAsync<DialogueModel>("dialogues"));
                    list.Insert(0, new NullDialogue());
                    break;
                case RPGResource.Item:
                    break;
                default:
                    break;
            }
            return list;
        }

        private interface INullResource { }

        private class NullQuest : QuestModel, INullResource
        {
            public NullQuest() => Id = -1;
            public override string ToString() => "None";
        }

        private class NullDialogue : DialogueModel, INullResource
        {
            public NullDialogue() => Id = -1;
            public override string ToString() => "None";
        }
    }
}
