using RPGDataEditor.Core.Connection;
using System;
using System.Windows;

namespace RPGDataEditor.Wpf.Controls
{
    public class SessionControl : ChangeableUserControl
    {
        private static readonly TypeSource[] sources = new TypeSource[] {
            new TypeSource("Ftp", typeof(FtpFileClient)),
            new TypeSource("Local", typeof(LocalFileClient)),
            //new TypeSource("Mssql", typeof(SqlClient))
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

        protected override object GetActualContentResource(Type type) => Application.Current.TryFindResource(type.Name + "ConnectionContent");

        protected override Type GetDataContextItemType()
        {
            if (DataContext is ISessionContext session)
            {
                return session.Client?.GetType().Name.Replace("ResourceClient", "");
            }
            return null;
        }
    }
}
