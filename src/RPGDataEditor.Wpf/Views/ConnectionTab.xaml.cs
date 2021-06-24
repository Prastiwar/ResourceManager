using Microsoft.Extensions.Configuration;
using ResourceManager;
using System.Windows.Controls;

namespace RPGDataEditor.Wpf.Views
{
    public partial class ConnectionTab : UserControl
    {
        public ConnectionTab() => InitializeComponent();

        private void SessionControl_TypeChange(object sender, Controls.ChangeableUserControl.ChangeTypeEventArgs e)
        {
            if (SessionControl.DataContext is IConfiguration configuration)
            {
                configuration.SetDataSource(e.TargetType.Name);
            }
        }
    }
}
