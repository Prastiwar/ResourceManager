using Microsoft.Extensions.Logging;
using Prism.Services.Dialogs;
using ResourceManager.DataSource;
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
        public ConnectionDialogViewModel(IDataSource dataSource, ILogger<ConnectionDialogViewModel> logger)
            : base(logger) => DataSource = dataSource;

        public override string Title => "Connection problem";

        protected IDataSource DataSource { get; }

        protected override Task InitializeAsync(IDialogParameters parameters)
        {
            DataSource.Monitor.Changed -= OnConnectionChanged;
            DataSource.Monitor.Changed += OnConnectionChanged;
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
                DataSource.Monitor.Changed -= OnConnectionChanged;
                Application.Current.Dispatcher.Invoke(() => Close(new RPGDataEditor.Mvvm.Navigation.DialogParametersBuilder().WithResult(true).BuildPrism()));
            }
        }

        private void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.System && e.SystemKey == Key.F4)
            {
                Application.Current.Shutdown();
                e.Handled = true;
            }
        }
    }
}
