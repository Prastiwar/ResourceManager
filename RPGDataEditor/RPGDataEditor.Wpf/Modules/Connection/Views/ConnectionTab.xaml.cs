using RPGDataEditor.Core.Mvvm;
using System.Windows.Controls;

namespace RPGDataEditor.Wpf.Connection.Views
{
    public partial class ConnectionTab : UserControl
    {
        public ConnectionTab() => InitializeComponent();

        private void PasswordBox_PasswordChanged(object sender, System.Windows.RoutedEventArgs e)
        {
            if (DataContext != null)
            {
                ((dynamic)FtpPasswordBox.DataContext).FtpPassword = FtpPasswordBox.Password;
            }
        }

        private void FtpPasswordBox_DataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            PasswordBox box = (PasswordBox)sender;
            if (box.DataContext is SessionContext context)
            {
                box.Password = context.FtpPassword;
            }
        }

        private void IsFtpCheckbox_DataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            CheckBox box = (CheckBox)sender;
            if (box.DataContext is SessionContext context)
            {
                box.IsChecked = context.IsFtp;
            }
        }
    }
}
