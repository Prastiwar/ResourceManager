using Microsoft.Extensions.Logging;
using ResourceManager.DataSource;
using ResourceManager.Extensions.Prism.Wpf.ViewModels;
using ResourceManager.Mvvm.Services;
using RpgDataEditor.Models;

namespace RpgDataEditor.Wpf.ViewModels
{
    public class QuestTabViewModel : CategoryModelsManagerViewModel<Quest>
    {
        public QuestTabViewModel(IViewService viewService, IDataSource dataSource, ILogger<QuestTabViewModel> logger)
            : base(viewService, dataSource, logger) { }
    }
}
