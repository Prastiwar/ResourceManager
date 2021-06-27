using Microsoft.Extensions.Configuration;
using ResourceManager.DataSource;
using System.Windows;

namespace ResourceManager.Wpf.Controls
{
    public class SessionControl : ChangeableUserControl
    {
        private static readonly TypeSource[] sources = new TypeSource[] {
            new TypeSource("Ftp", typeof(IConfiguration)),
            new TypeSource("Local", typeof(IConfiguration)),
            new TypeSource("Sql", typeof(IConfiguration))
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
            if (DataContext is IConfiguration configuration)
            {
                return new TypeSource(configuration[DataSourceExtensions.NameKey], DataContext.GetType());
            }
            return null;
        }
    }
}
