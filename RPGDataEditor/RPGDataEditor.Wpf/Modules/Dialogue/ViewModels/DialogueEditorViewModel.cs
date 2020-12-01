using Prism.Services.Dialogs;
using RPGDataEditor.Core.Models;
using RPGDataEditor.Core.Mvvm;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RPGDataEditor.Wpf.Dialogue.ViewModels
{
    public class DialogueEditorViewModel : ModelDialogViewModel<DialogueModel>
    {
        public DialogueEditorViewModel(ViewModelContext context) : base(context) { }

        public override string Title => "Dialogue Editor";

        public ICommand AddOptionCommand => Commands.AddListItemLiCommand(() => Model.Options);

        public ICommand RemoveOptionCommand => Commands.RemoveListItemLiCommand(() => Model.Options);

        public ICommand AddRequirementCommand => Commands.AddListItemLiCommand(() => Model.Requirements, () => new DialogueRequirement());

        public ICommand RemoveRequirementCommand => Commands.RemoveListItemLiCommand(() => Model.Requirements);

        public int OptionsCount => Model == null ? 0 : Model.Options.Count;

        public int RequirementsCount => Model == null ? 0 : Model.Requirements.Count;

        protected override Task InitializeAsync(IDialogParameters parameters)
        {
            base.InitializeAsync(parameters);
            if (Model.Options is INotifyCollectionChanged optionsNotifier)
            {
                optionsNotifier.CollectionChanged += Options_CollectionChanged;
            }
            if (Model.Requirements is INotifyCollectionChanged requirementsNotifier)
            {
                requirementsNotifier.CollectionChanged += Requirements_CollectionChanged;
            }
            return Task.CompletedTask;
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.PropertyName == nameof(Model))
            {
                RaisePropertyChanged(nameof(OptionsCount));
                RaisePropertyChanged(nameof(RequirementsCount));
            }
        }

        private void Options_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e) => RaisePropertyChanged(nameof(OptionsCount));

        private void Requirements_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e) => RaisePropertyChanged(nameof(RequirementsCount));
    }
}
