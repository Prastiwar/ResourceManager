using RPGDataEditor.Core.Mvvm;
using System.Windows;

namespace RPGDataEditor.Wpf.Controls
{
    public class SessionControl : ChangeableUserControl
    {
        private static readonly string[] types = new string[] {
            "Ftp",
            "Local",
            //"Mssql"
        };

        protected override void OnTemplateApplied()
        {
            if (GetBindingExpression(TypesSourceProperty) == null)
            {
                TypesSource = GetConnectionTypes();
            }
            base.OnTemplateApplied();
        }

        protected virtual string[] GetConnectionTypes() => types;

        protected override object GetActualContentResource(string name) => Application.Current.TryFindResource(name + "ConnectionContent");

        protected override string GetDataContextItemName()
        {
            if (DataContext is ISessionContext session)
            {
                return session.Client?.GetType().Name.Replace("ResourceClient", "");
            }
            return null;
        }
    }
}
