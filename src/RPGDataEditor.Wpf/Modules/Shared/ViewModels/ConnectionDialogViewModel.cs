using Prism.Services.Dialogs;
using RPGDataEditor.Connection;
using RPGDataEditor.Core;
using RPGDataEditor.Extensions.Prism.Wpf;
using RPGDataEditor.Mvvm;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace RPGDataEditor.Wpf.ViewModels
{
    public class ConnectionDialogViewModel : DialogViewModelBase
    {
        public ConnectionDialogViewModel(ViewModelContext context, IConnectionChecker connectionChecker) 
            : base(context) => ConnectionChecker = connectionChecker;

        public override string Title => "Connection problem";

        protected IConnectionChecker ConnectionChecker { get; }

        protected override Task InitializeAsync(IDialogParameters parameters)
        {
            ConnectionChecker.Changed -= OnConnectionChanged;
            ConnectionChecker.Changed += OnConnectionChanged;
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
                ConnectionChecker.Changed -= OnConnectionChanged;
                Application.Current.Dispatcher.Invoke(() => Close(new RPGDataEditor.Mvvm.Navigation.DialogParametersBuilder().WithResult(true).BuildPrism()));
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
