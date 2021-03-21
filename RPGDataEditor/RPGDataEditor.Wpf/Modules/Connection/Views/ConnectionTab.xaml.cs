using RPGDataEditor.Core.Connection;
using RPGDataEditor.Core.Mvvm;
using System.Windows;
using System.Windows.Controls;

namespace RPGDataEditor.Wpf.Connection.Views
{
    public partial class ConnectionTab : UserControl
    {
        public ConnectionTab() => InitializeComponent();

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (FtpPasswordBox.DataContext != null)
            {
                ((dynamic)FtpPasswordBox.DataContext).Password = FtpPasswordBox.Password;
            }
        }

        private void FtpPasswordBox_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != null)
            {
                PasswordBox box = (PasswordBox)sender;
                if (box.DataContext != null)
                {
                    try
                    {
                        box.Password = ((dynamic)box.DataContext).Password;
                    }
                    catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException ex)
                    {
                        System.Diagnostics.Debug.Print(ex.Message);
                    }
                }
            }
        }

        private void ConnectionComboBox_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            ComboBox box = (ComboBox)sender;
            if (box.DataContext is SessionContext context)
            {
                box.SelectionChanged -= ConnectionComboBox_SelectionChanged;
                if (context.Client is ExplorerResourceClient)
                {
                    box.SelectedIndex = 0;
                }
                else if (context.Client is FtpResourceClient)
                {
                    box.SelectedIndex = 1;
                }
                else if (context.Client is MssqlResourceClient)
                {
                    box.SelectedIndex = 2;
                }
                box.SelectionChanged += ConnectionComboBox_SelectionChanged;
            }
        }

        private void ConnectionComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                if (e.AddedItems[0] is ComboBoxItem selected)
                {
                    ComboBox comboBox = (ComboBox)sender;
                    if (comboBox.DataContext is SessionContext context)
                    {
                        context.SetConnection(selected.Name);
                    }
                }
            }
        }
    }
}
