using RPGDataEditor.Connection;
using System.Windows;

namespace RPGDataEditor.Wpf.Controls
{
    public class SessionControl : ChangeableUserControl
    {
        private static readonly TypeSource[] sources = new TypeSource[] {
            new TypeSource("Ftp", typeof(ConnectionSettings)),
            new TypeSource("Local", typeof(ConnectionSettings)),
            new TypeSource("Mssql", typeof(ConnectionSettings))
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
            if (DataContext is IConnectionSettings settings)
            {
                return new TypeSource(settings.Get(nameof(ConnectionSettings.Type)).ToString(), DataContext.GetType());
            }
            return null;
        }
    }
}
