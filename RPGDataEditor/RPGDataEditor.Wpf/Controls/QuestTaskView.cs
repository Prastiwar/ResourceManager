using System.Windows;

namespace RPGDataEditor.Wpf.Controls
{
    public class QuestTaskView : ChangeableUserControl
    {
        private static readonly string[] types = new string[] {
            "EntityInteract",
            "Kill",
            "Reach",
            "LeftBlockInteract",
            "RightBlockInteract",
            "RightItemInteract",
            "Dialogue"
        };

        protected override void OnTemplateApplied()
        {
            if (GetBindingExpression(TypesSourceProperty) == null)
            {
                TypesSource = GetTasksNames();
            }
            base.OnTemplateApplied();
        }

        protected virtual string[] GetTasksNames() => types;

        protected override object GetActualContentResource(string name) => Application.Current.TryFindResource(name + "QuestTaskContent");

        protected override string GetDataContextItemName() => DataContext.GetType().Name.Replace("QuestTask", "");
    }
}
