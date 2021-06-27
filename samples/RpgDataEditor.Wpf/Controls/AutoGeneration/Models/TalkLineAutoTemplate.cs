using RpgDataEditor.Models;
using ResourceManager.Wpf.Controls;
using System.Windows;
using System.Windows.Controls;

namespace RpgDataEditor.Wpf.Controls
{
    public class TalkLineAutoTemplate : AutoTemplate<TalkLine>
    {
        public override DependencyObject LoadContent(object context, TemplateOptions options)
        {
            StackPanel panel = new StackPanel() {
                Orientation = Orientation.Horizontal
            };
            panel.Children.Add(new AutoControl() { PropertyName = nameof(TalkLine.Text) });
            panel.Children.Add(new AutoControl() { PropertyName = nameof(TalkLine.SoundId) });
            return panel;
        }
    }
}
