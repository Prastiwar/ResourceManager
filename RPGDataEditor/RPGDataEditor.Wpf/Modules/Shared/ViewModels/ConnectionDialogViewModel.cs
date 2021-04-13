using Prism.Services.Dialogs;
using RPGDataEditor.Core;
using RPGDataEditor.Core.Mvvm;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace RPGDataEditor.Wpf.ViewModels
{
    public class ConnectionDialogViewModel : DialogViewModelBase
    {
        public ConnectionDialogViewModel(ViewModelContext context) : base(context) { }

        public override string Title => "Connection problem";

        protected override Task InitializeAsync(IDialogParameters parameters)
        {
            Context.ConnectionService.ConnectionChanged -= OnConnectionChanged;
            Context.ConnectionService.ConnectionChanged += OnConnectionChanged;
            Window window = Application.Current.FindWindow(win => win.DataContext == this);
            if (window != null)
            {
                window.PreviewKeyDown += OnPreviewKeyDown;
            }
            return Task.CompletedTask;
        }

        private void OnConnectionChanged(object sender, bool hasConnection)
        {
            if (hasConnection)
            {
                Context.ConnectionService.ConnectionChanged -= OnConnectionChanged;
                Application.Current.Dispatcher.Invoke(() => Close(new DialogParametersBuilder().WithResult(true).Build()));
            }
        }

        private void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.System && e.SystemKey == Key.F4)
            {
                e.Handled = true;
            }
        }
    }
}
