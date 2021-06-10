using Microsoft.Extensions.Logging;
using ResourceManager.Data;
using ResourceManager.DataSource;
using ResourceManager.Services;
using RPGDataEditor.Mvvm.Services;

namespace RPGDataEditor.Minecraft.Wpf.Npc.ViewModels
{
    public class NpcTabViewModel : RPGDataEditor.Wpf.Npc.ViewModels.NpcTabViewModel
    {
        public NpcTabViewModel(IResourceDescriptorService resourceService, IViewService viewService, IDataSource dataSource, ILogger<NpcTabViewModel> logger) 
            : base(resourceService, viewService, dataSource, logger) { }

        protected override RPGDataEditor.Models.Npc CreateResource(PresentableData model) => new Models.Npc() {
            Id = model.Id,
            Name = model.Name
        };
    }
}
