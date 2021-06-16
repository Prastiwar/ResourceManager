using RPGDataEditor.Models;
using System.Windows;

namespace RPGDataEditor.Wpf.Controls.Changeable
{
    public class NpcJobView : ChangeableUserControl
    {
        private static readonly TypeSource[] sources = new TypeSource[] {
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
                return null;
            }
            return new TypeSource(DataContext.GetType().Name.Replace("NpcJob", ""), DataContext.GetType());
        }
    }
}
