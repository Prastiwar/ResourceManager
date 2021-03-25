using Prism;
using RPGDataEditor.Core.Models;
using RPGDataEditor.Core.Providers;
using RPGDataEditor.Wpf.Controls;
using System.Windows;

namespace RPGDataEditor.Wpf
{
    public static class ViewsExtensions
    {
        public static PlayerRequirementModel CreateRequirement(this ChangeableUserControl.ChangeTypeEventArgs e)
        {
            if (Application.Current is PrismApplicationBase prismApp)
            {
                if (prismApp.Container.Resolve(typeof(IModelProvider<PlayerRequirementModel>)) is IModelProvider<PlayerRequirementModel> provider)
                {
                    return provider.CreateModel(e.TargetType);
                }
            }
            return null;
        }
    }
}
