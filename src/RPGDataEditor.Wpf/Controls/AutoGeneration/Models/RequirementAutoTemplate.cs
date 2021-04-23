using RPGDataEditor.Models;
using System.Reflection;
using System.Windows;

namespace RPGDataEditor.Wpf.Controls.AutoGeneration.Models
{
    public class RequirementAutoTemplate : AutoTemplate<Requirement>
    {
        public override DependencyObject LoadContent(PropertyInfo info = null)
        {
            RequirementView view = new RequirementView();
            AutoControl.SetPreserveDataContext(view, false);
            view.TypeChange += View_TypeChange;
            return view;
        }

        private void View_TypeChange(object sender, ChangeableUserControl.ChangeTypeEventArgs e) => e.ChangeType<Requirement>(sender);
    }
}
