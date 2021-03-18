using RPGDataEditor.Core.Models;
using System.Windows;
using System.Windows.Controls;

namespace RPGDataEditor.Wpf.Views
{
    public partial class BlockPosField : UserControl
    {
        public BlockPosField() => InitializeComponent();

        public static DependencyProperty OrientationProperty =
            DependencyProperty.Register(nameof(Orientation), typeof(Orientation), typeof(BlockPosField), new PropertyMetadata(Orientation.Horizontal, OnOrientationChanged));
        private static void OnOrientationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as BlockPosField).FieldPanel.Orientation = (Orientation)e.NewValue;
        public Orientation Orientation {
            get => (Orientation)GetValue(OrientationProperty);
            set => SetValue(OrientationProperty, value);
        }

        public static DependencyProperty PositionProperty = DependencyProperty.Register(nameof(Position), typeof(Position), typeof(BlockPosField), new PropertyMetadata(new Position()));
        public Position Position {
            get => (Position)GetValue(PositionProperty);
            set => SetValue(PositionProperty, value);
        }

        public static DependencyProperty HeaderTextProperty = DependencyProperty.Register(nameof(HeaderText), typeof(string), typeof(BlockPosField), new PropertyMetadata(null));
        public string HeaderText {
            get => (string)GetValue(HeaderTextProperty);
            set => SetValue(HeaderTextProperty, value);
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.Property == OrientationProperty)
            {
                FieldPanel.Orientation = (Orientation)e.NewValue;
            }
        }
    }
}
