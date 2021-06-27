using Microsoft.Extensions.Logging;
using ResourceManager.DataSource;
using RPGDataEditor.Extensions.Prism.Wpf.ViewModels;
using RPGDataEditor.Mvvm.Services;
using RPGDataEditor.Sample.Models;

namespace RPGDataEditor.Sample.Wpf.ViewModels
{
    public class DialogueTabViewModel : CategoryModelsManagerViewModel<Dialogue>
    {
        public DialogueTabViewModel(IViewService viewService, IDataSource dataSource, ILogger<DialogueTabViewModel> logger)
            : base(viewService, dataSource, logger) { }
    }
}
