using System;

namespace RPGDataEditor.Wpf
{
    internal class Program
    {
        [STAThread]
        internal static void Main()
        {
            RPGDataEditorApp application = new RPGDataEditorApp();
            application.InitializeComponent();
            application.Run();
        }
    }
}