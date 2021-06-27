using Microsoft.Extensions.Logging;
using ResourceManager.DataSource;
using ResourceManager.Extensions.Prism.Wpf.ViewModels;
using ResourceManager.Mvvm.Services;
using RpgDataEditor.Models;

namespace RpgDataEditor.Wpf.ViewModels
{
    public class NpcTabViewModel : ModelsManagerViewModel<Npc>
    {
        public NpcTabViewModel(IViewService viewService, IDataSource dataSource, ILogger<NpcTabViewModel> logger)
            : base(viewService, dataSource, logger) { }
    }
}
