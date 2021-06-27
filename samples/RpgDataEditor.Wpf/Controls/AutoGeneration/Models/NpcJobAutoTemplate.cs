using RpgDataEditor.Models;
using ResourceManager.Wpf;
using ResourceManager.Wpf.Controls;
using System.Windows;

namespace RpgDataEditor.Wpf.Controls
{
    public class NpcJobAutoTemplate : AutoTemplate<NpcJob>
    {
        public override DependencyObject LoadContent(object context, TemplateOptions options)
        {
            NpcJobView jobView = new NpcJobView() {
                HintText = "Job Type"
            };
            AutoControl.SetPreserveDataContext(jobView, false);
            jobView.TypeChange += JobView_TypeChange;
            return jobView;
        }

        private void JobView_TypeChange(object sender, ChangeableUserControl.ChangeTypeEventArgs e) => e.ChangeType<NpcJob>(sender);
    }
}
