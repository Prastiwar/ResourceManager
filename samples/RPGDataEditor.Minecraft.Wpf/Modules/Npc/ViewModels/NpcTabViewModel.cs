using MediatR;
using Microsoft.Extensions.Logging;
using ResourceManager.Data;

namespace RPGDataEditor.Minecraft.Wpf.Npc.ViewModels
{
    public class NpcTabViewModel : RPGDataEditor.Wpf.Npc.ViewModels.NpcTabViewModel
    {
        public NpcTabViewModel(IMediator mediator, ILogger<NpcTabViewModel> logger) : base(mediator, logger) { }

        protected override RPGDataEditor.Models.Npc CreateResource(PresentableData model) => new Models.Npc() {
            Id = model.Id,
            Name = model.Name
        };
    }
}
