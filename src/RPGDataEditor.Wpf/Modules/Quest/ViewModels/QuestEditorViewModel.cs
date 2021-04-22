using RPGDataEditor.Mvvm;

namespace RPGDataEditor.Wpf.Quest.ViewModels
{
    public class QuestEditorViewModel : ModelDialogViewModel<Models.Quest>
    {
        public QuestEditorViewModel(ViewModelContext context) : base(context) { }

        public override string Title => "Quest Editor";
    }
}
