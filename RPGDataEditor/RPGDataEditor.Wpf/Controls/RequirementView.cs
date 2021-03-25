using System.Windows;

namespace RPGDataEditor.Wpf.Controls
{
    public class RequirementView : ChangeableUserControl
    {
        private static readonly string[] types = new string[] { "Dialogue", "Quest", "Item" };

        protected override void OnTemplateApplied()
        {
            if (GetBindingExpression(TypesSourceProperty) == null)
            {
                TypesSource = GetRequirementNames();
            }
            base.OnTemplateApplied();
        }

        protected virtual string[] GetRequirementNames() => types;

        protected override object GetActualContentResource(string name) => Application.Current.TryFindResource(name + "RequirementContent");

        protected override string GetDataContextItemName() => DataContext.GetType().Name.Replace("Requirement", "");
    }
}
