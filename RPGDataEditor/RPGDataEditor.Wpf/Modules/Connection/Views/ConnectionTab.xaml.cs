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
    }
}
