using Microsoft.Extensions.Logging;
using ResourceManager.Data;
using ResourceManager.DataSource;
using RPGDataEditor.Extensions.Prism.Wpf.ViewModels;
using RPGDataEditor.Models;
using RPGDataEditor.Mvvm.Services;

namespace RPGDataEditor.Wpf.Npc.ViewModels
{
    public class NpcTabViewModel : PresentableDataViewModel<Models.Npc>
    {
        public NpcTabViewModel(IViewService viewService, IDataSource dataSource, ILogger<NpcTabViewModel> logger)
            : base(viewService, dataSource, logger) { }

        protected override PresentableData CreateModelInstance() => new PresentableNpc();

        protected override Models.Npc CreateResource(PresentableData model) => new Models.Npc() {
            Id = model.Id,
            Name = model.Name
        };
    }
}
