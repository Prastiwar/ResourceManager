using Prism.Services.Dialogs;
using RPGDataEditor.Core;
using RPGDataEditor.Core.Models;
using RPGDataEditor.Core.Mvvm;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RPGDataEditor.Wpf.ViewModels
{
    public class PickerDialogViewModel : DialogViewModelBase
    {
        public PickerDialogViewModel(ViewModelContext context) : base(context) { }

        public override string Title => "Resource Picker";

        private object model;
        public object Model {
            get => model;
            set => SetProperty(ref model, value);
        }

        private IList<object> models;
        public IList<object> Models {
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
            List<object> list = new List<object>();
            switch (resource)
            {
                case RPGResource.Quest:
                    list = new List<object>(await Context.Session.LoadAsync<QuestModel>("quests"));
                    list.Insert(0, new NullQuest());
                    break;
                case RPGResource.Dialogue:
                    list = new List<object>(await Context.Session.LoadAsync<DialogueModel>("dialogues"));
                    list.Insert(0, new NullDialogue());
                    break;
                case RPGResource.Item:
                    break;
                default:
                    break;
            }
            Comparer<object> comparer = Comparer<object>.Create((x, y) => {
                if (x is IIdentifiable identifiableX && y is IIdentifiable identifiableY)
                {
                    int xId = identifiableX.GetId();
                    int yId = identifiableY.GetId();
                    if (xId == yId)
                    {
                        return 0;
                    }
                    else if (xId < yId)
                    {
                        return -1;
                    }
                    return 1;
                }
                return 0;
            });
            list.Sort(comparer);
            Models = list;
        }

        private interface INullResource { }

        private class NullQuest : QuestModel, INullResource
        {
            public override string ToString() => "None";
        }

        private class NullDialogue : DialogueModel, INullResource
        {
            public override string ToString() => "None";
        }
    }
}
