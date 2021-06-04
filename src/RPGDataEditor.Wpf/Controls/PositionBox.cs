using RPGDataEditor.Models;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace RPGDataEditor.Wpf.Controls
{
    public class PositionBox : UserControl
    {
        public static DependencyProperty OrientationProperty =
            DependencyProperty.Register(nameof(Orientation), typeof(Orientation), typeof(PositionBox), new PropertyMetadata(Orientation.Horizontal));
        public Orientation Orientation {
            get => (Orientation)GetValue(OrientationProperty);
            set => SetValue(OrientationProperty, value);
        }

        public static DependencyProperty PositionProperty = DependencyProperty.Register(nameof(Position), typeof(Position), typeof(PositionBox),
            new FrameworkPropertyMetadata(new Position(), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnPositionChanged));

        private static void OnPositionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == null)
            {
                Position pos = new Position();
                (d as PositionBox).Position = pos;
                BindingExpression exp = BindingOperations.GetBindingExpression(d, PositionProperty);
                // TODO: Find out why it doesnt update in TwoWay mode without this hack
                if (exp.ParentBinding.Mode == BindingMode.TwoWay)
                {
                    exp.ResolvedSource.GetType().GetProperty(exp.ResolvedSourcePropertyName, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance).SetValue(exp.ResolvedSource, pos);
                }
            }
        }

        public Position Position {
            get => (Position)GetValue(PositionProperty);
            set => SetValue(PositionProperty, value);
        }

        public static DependencyProperty HeaderTextProperty = DependencyProperty.Register(nameof(HeaderText), typeof(string), typeof(PositionBox), new PropertyMetadata(null));
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
