using RPGDataEditor.Core.Connection;
using System.Windows;

namespace RPGDataEditor.Wpf.Controls
{
    public class SessionControl : ChangeableUserControl
    {
        private static readonly TypeSource[] sources = new TypeSource[] {
            new TypeSource("Ftp", typeof(FtpFileClient)),
            new TypeSource("Local", typeof(LocalFileClient)),
            new TypeSource("Mssql", typeof(SqlClient))
        };

        protected override void OnTemplateApplied()
        {
            if (GetBindingExpression(TypesSourceProperty) == null)
            {
                TypesSource = GetSources();
            }
            base.OnTemplateApplied();
        }

        protected virtual TypeSource[] GetSources() => sources;

        protected override object GetActualContentResource(TypeSource type) => Application.Current.TryFindResource(type.Name + "ConnectionContent");

        protected override TypeSource GetDataContextTypeSource()
        {
            if (DataContext == null)
            {
                return null;
            }
            return new TypeSource(DataContext.GetType().Name.Replace("Client", ""), DataContext.GetType());
        }
    }
}
