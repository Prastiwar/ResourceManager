using Prism;
using RPGDataEditor.Core.Models;
using RPGDataEditor.Core.Providers;
using RPGDataEditor.Wpf.Controls;
using System.Windows;

namespace RPGDataEditor.Wpf
{
    public static class ViewsExtensions
    {
        public static PlayerRequirementModel CreateRequirement(this RequirementView.ChangeTypeEventArgs e)
        {
            if (Application.Current is PrismApplicationBase prismApp)
            {
                if (prismApp.Container.Resolve(typeof(IRequirementProvider)) is IRequirementProvider provider)
                {
                    return provider.CreateRequirement(e.TargetType);
                }
            }
            return null;
        }
    }
}
