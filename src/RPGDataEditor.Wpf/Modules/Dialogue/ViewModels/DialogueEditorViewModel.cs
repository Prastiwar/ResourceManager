using RPGDataEditor.Mvvm;

namespace RPGDataEditor.Wpf.Dialogue.ViewModels
{
    public class DialogueEditorViewModel : ModelDialogViewModel<Models.Dialogue>
    {
        public DialogueEditorViewModel(ViewModelContext context) : base(context) { }

        public override string Title => "Dialogue Editor";
    }
}
