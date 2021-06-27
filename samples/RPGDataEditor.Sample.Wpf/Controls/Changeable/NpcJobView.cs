using RPGDataEditor.Sample.Models;
using RPGDataEditor.Wpf.Controls;
using System.Windows;

namespace RPGDataEditor.Sample.Wpf.Controls
{
    public class NpcJobView : ChangeableUserControl
    {
        private static readonly TypeSource[] sources = new TypeSource[] {
            new TypeSource("None", null),
            new TypeSource("Trader", typeof(TraderNpcJob)),
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

        protected override object GetActualContentResource(TypeSource type) => Application.Current.TryFindResource(type.Name + "NpcJobContent");

        protected override TypeSource GetDataContextTypeSource()
        {
            if (DataContext == null)
            {
                return sources[0];
            }
            return new TypeSource(DataContext.GetType().Name.Replace("NpcJob", ""), DataContext.GetType());
        }
    }
}
