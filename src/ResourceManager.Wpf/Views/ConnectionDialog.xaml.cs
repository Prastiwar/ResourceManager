using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace ResourceManager.Wpf.Views
{
    public partial class ConnectionDialog : UserControl
    {
        public ConnectionDialog()
        {
            InitializeComponent();
            AnimateButton(CircleButton);
        }

        private void AnimateButton(Button button)
        {
            RotateTransform transform = new RotateTransform();
            button.RenderTransform = transform;
            DoubleAnimation rotateAnimation = new DoubleAnimation(0, 360, new Duration(TimeSpan.FromSeconds(1))) {
                RepeatBehavior = RepeatBehavior.Forever
            };
            transform.BeginAnimation(RotateTransform.AngleProperty, rotateAnimation);
        }
    }
}
