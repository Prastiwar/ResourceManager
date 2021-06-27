using Microsoft.Extensions.Logging;
using ResourceManager.DataSource;
using ResourceManager.Extensions.Prism.Wpf.ViewModels;
using ResourceManager.Mvvm.Services;
using RpgDataEditor.Models;

namespace RpgDataEditor.Wpf.ViewModels
{
    public class DialogueTabViewModel : CategoryModelsManagerViewModel<Dialogue>
    {
        public DialogueTabViewModel(IViewService viewService, IDataSource dataSource, ILogger<DialogueTabViewModel> logger)
            : base(viewService, dataSource, logger) { }
    }
}
