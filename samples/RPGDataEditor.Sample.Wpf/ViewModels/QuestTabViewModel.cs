using Microsoft.Extensions.Logging;
using ResourceManager.DataSource;
using RPGDataEditor.Extensions.Prism.Wpf.ViewModels;
using RPGDataEditor.Mvvm.Services;
using RPGDataEditor.Sample.Models;

namespace RPGDataEditor.Sample.Wpf.ViewModels
{
    public class QuestTabViewModel : CategoryModelsManagerViewModel<Quest>
    {
        public QuestTabViewModel(IViewService viewService, IDataSource dataSource, ILogger<QuestTabViewModel> logger)
            : base(viewService, dataSource, logger) { }
    }
}
