using RPGDataEditor.Core.Models;
using RPGDataEditor.Core.Mvvm;

namespace RPGDataEditor.Wpf.Quest.ViewModels
{
    public class QuestTabViewModel : CategorizedTabViewModel<QuestModel>
    {
        public QuestTabViewModel(ViewModelContext context) : base(context) { }

        protected override string RelativePath => "quests";
    }
}
