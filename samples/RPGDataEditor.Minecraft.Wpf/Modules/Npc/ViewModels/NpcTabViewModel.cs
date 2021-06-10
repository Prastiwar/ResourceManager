using Microsoft.Extensions.Logging;
using ResourceManager.Data;
using ResourceManager.DataSource;
using RPGDataEditor.Mvvm.Services;

namespace RPGDataEditor.Minecraft.Wpf.Npc.ViewModels
{
    public class NpcTabViewModel : RPGDataEditor.Wpf.Npc.ViewModels.NpcTabViewModel
    {
        public NpcTabViewModel(IViewService viewService, IDataSource dataSource, ILogger<NpcTabViewModel> logger)
            : base(viewService, dataSource, logger) { }

        protected override RPGDataEditor.Models.Npc CreateResource(PresentableData model) => new Models.Npc() {
            Id = model.Id,
            Name = model.Name
        };
    }
}
