using RPGDataEditor.Sample.Models;
using RPGDataEditor.Wpf.Controls;
using System.Windows;
using System.Windows.Controls;

namespace RPGDataEditor.Sample.Wpf.Controls
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
