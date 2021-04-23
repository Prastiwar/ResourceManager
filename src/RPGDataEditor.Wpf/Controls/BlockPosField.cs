using RPGDataEditor.Models;
using System.Windows;
using System.Windows.Controls;

namespace RPGDataEditor.Wpf.Controls
{
    public class BlockPosField : UserControl
    {
        public static DependencyProperty OrientationProperty =
            DependencyProperty.Register(nameof(Orientation), typeof(Orientation), typeof(BlockPosField), new PropertyMetadata(Orientation.Horizontal));
        public Orientation Orientation {
            get => (Orientation)GetValue(OrientationProperty);
            set => SetValue(OrientationProperty, value);
        }

        public static DependencyProperty PositionProperty = DependencyProperty.Register(nameof(Position), typeof(Position), typeof(BlockPosField));
        public Position Position {
            get => (Position)GetValue(PositionProperty);
            set => SetValue(PositionProperty, value);
        }

        public static DependencyProperty HeaderTextProperty = DependencyProperty.Register(nameof(HeaderText), typeof(string), typeof(BlockPosField), new PropertyMetadata(null));
        public string HeaderText {
            get => (string)GetValue(HeaderTextProperty);
            set => SetValue(HeaderTextProperty, value);
        }

        private StackPanel panel;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            panel = Template.FindName("FieldPanel", this) as StackPanel;
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.Property == OrientationProperty)
            {
                panel.Orientation = (Orientation)e.NewValue;
            }
        }
    }
}
