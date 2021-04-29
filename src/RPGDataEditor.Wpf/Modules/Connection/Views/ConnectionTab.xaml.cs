using RPGDataEditor.Core.Connection;
using System.Windows.Controls;

namespace RPGDataEditor.Wpf.Connection.Views
{
    public partial class ConnectionTab : UserControl
    {
        public ConnectionTab() => InitializeComponent();

        private void SessionControl_TypeChange(object sender, Controls.ChangeableUserControl.ChangeTypeEventArgs e)
        {
            if (SessionControl.DataContext is ConnectionSettings settings)
            {
                settings.Config = e.CreateModel<IConnectionConfig>();
            }
        }
    }
}
