using RPGDataEditor.Core.Models;
using RPGDataEditor.Core.Mvvm;
using System.Windows.Controls;

namespace RPGDataEditor.Wpf.Quest.Views
{
    public partial class QuestEditor : UserControl
    {
        public QuestEditor() => InitializeComponent();

        private void CompletionQuestTaskView_TypeChange(object sender, Controls.ChangeableUserControl.ChangeTypeEventArgs e)
        {
            if (DataContext is ModelDialogViewModel<QuestModel> vm)
            {
                QuestTask newModel = e.CreateModel<QuestTask>();
                if (newModel != null)
                {
                    vm.Model.CompletionTask = newModel;
                }
            }
        }
    }
}
