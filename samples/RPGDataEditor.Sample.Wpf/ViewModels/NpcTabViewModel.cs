using Microsoft.Extensions.Logging;
using ResourceManager.Data;
using ResourceManager.DataSource;
using RPGDataEditor.Extensions.Prism.Wpf.ViewModels;
using RPGDataEditor.Sample.Models;
using RPGDataEditor.Mvvm.Services;

namespace RPGDataEditor.Sample.Wpf.ViewModels
{
    public class NpcTabViewModel : PresentableDataViewModel<Npc>
    {
        public NpcTabViewModel(IViewService viewService, IDataSource dataSource, ILogger<NpcTabViewModel> logger)
            : base(viewService, dataSource, logger) { }

        protected override PresentableData CreateModelInstance() => new PresentableNpc();

        protected override Npc CreateResource(PresentableData model) => new Npc() {
            Id = model.Id,
            Name = model.Name
        };
    }
}
