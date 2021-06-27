using ResourceManager.Mvvm;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace ResourceManager.Wpf.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow() => InitializeComponent();

        private bool canRefresh = true;

        private async void Refresh_Click(object sender, RoutedEventArgs e)
        {
            if (!(MainTabControl.SelectedContent is FrameworkElement currentContent) || !(currentContent.DataContext is ScreenViewModel vm) || !canRefresh)
            {
                return;
            }
            AttachProperties.SetIsLoading(MainTabControl, true);
            canRefresh = false;
            Button button = (Button)sender;
            RotateTransform transform = new RotateTransform();
            button.RenderTransform = transform;
            DoubleAnimation rotateAnimation = new DoubleAnimation(0, 360, new Duration(TimeSpan.FromSeconds(1))) {
                RepeatBehavior = RepeatBehavior.Forever
            };
            transform.BeginAnimation(RotateTransform.AngleProperty, rotateAnimation);
            await vm.Refresh();
            double currentAngle = transform.Angle;
            rotateAnimation = new DoubleAnimation(currentAngle, 360, new Duration(TimeSpan.FromSeconds(1)));
            rotateAnimation.Completed += RotateAnimation_Completed;
            transform.BeginAnimation(RotateTransform.AngleProperty, rotateAnimation);
            AttachProperties.SetIsLoading(MainTabControl, false);
        }

        private void RotateAnimation_Completed(object sender, EventArgs e) => canRefresh = true;
    }
}
