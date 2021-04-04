using RPGDataEditor.Core.Models;
using RPGDataEditor.Core.Mvvm;

namespace RPGDataEditor.Wpf.Quest.ViewModels
{
    public class QuestEditorViewModel : ModelDialogViewModel<QuestModel>
    {
        public QuestEditorViewModel(ViewModelContext context) : base(context) { }

        public override string Title => "Quest Editor";
    }
}
