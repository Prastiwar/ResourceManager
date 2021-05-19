using MediatR;
using Microsoft.Extensions.Logging;
using ResourceManager.Data;
using RPGDataEditor.Models;
using RPGDataEditor.Mvvm;

namespace RPGDataEditor.Wpf.Npc.ViewModels
{
    public class NpcTabViewModel : PresentableDataViewModel<Models.Npc>
    {
        public NpcTabViewModel(IMediator mediator, ILogger<NpcTabViewModel> logger) : base(mediator, logger) { }

        protected override PresentableData CreateModelInstance() => new PresentableNpc();

        protected override Models.Npc CreateResource(PresentableData model) => new Models.Npc() {
            Id = model.Id,
            Name = model.Name
        };
    }
}
