﻿using RPGDataEditor.Mvvm;
using System.Windows.Controls;

namespace RPGDataEditor.Wpf.Connection.Views
{
    public partial class ConnectionTab : UserControl
    {
        public ConnectionTab() => InitializeComponent();

        private void SessionControl_TypeChange(object sender, Controls.ChangeableUserControl.ChangeTypeEventArgs e)
        {
            //e.TargetType.Type
            //if (SessionControl.DataContext is ISessionContext context)
            //{
            //    context.SetConnection(e.TargetType);
            //}
        }
    }
}
