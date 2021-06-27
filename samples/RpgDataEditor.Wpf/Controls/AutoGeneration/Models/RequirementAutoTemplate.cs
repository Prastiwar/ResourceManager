using RpgDataEditor.Models;
using ResourceManager.Wpf;
using ResourceManager.Wpf.Controls;
using System.Windows;

namespace RpgDataEditor.Wpf.Controls
{
    public class RequirementAutoTemplate : AutoTemplate<Requirement>
    {
        public override DependencyObject LoadContent(object context, TemplateOptions options)
        {
            RequirementView view = new RequirementView();
            AutoControl.SetPreserveDataContext(view, false);
            view.TypeChange += View_TypeChange;
            return view;
        }

        private void View_TypeChange(object sender, ChangeableUserControl.ChangeTypeEventArgs e) => e.ChangeType<Requirement>(sender);
    }
}
