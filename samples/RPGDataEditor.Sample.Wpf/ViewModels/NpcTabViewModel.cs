using Microsoft.Extensions.Logging;
using ResourceManager.DataSource;
using RPGDataEditor.Extensions.Prism.Wpf.ViewModels;
using RPGDataEditor.Mvvm.Services;
using RPGDataEditor.Sample.Models;

namespace RPGDataEditor.Sample.Wpf.ViewModels
{
    public class NpcTabViewModel : ModelsManagerViewModel<Npc>
    {
        public NpcTabViewModel(IViewService viewService, IDataSource dataSource, ILogger<NpcTabViewModel> logger)
            : base(viewService, dataSource, logger) { }
    }
}
