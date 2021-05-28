using MediatR;
using Microsoft.Extensions.Logging;
using Prism.Commands;
using ResourceManager.Data;
using RPGDataEditor.Models;
using RPGDataEditor.Mvvm;
using System.Windows.Input;

namespace RPGDataEditor.Wpf.Npc.ViewModels
{
    public class NpcTabViewModel : PresentableDataViewModel<Models.Npc>
    {
        public NpcTabViewModel(IMediator mediator, ILogger<NpcTabViewModel> logger) : base(mediator, logger) { }

        public ICommand openEditorCommand;
        public ICommand OpenEditorCommand => openEditorCommand ??= new DelegateCommand<PresentableData>(async presentable => await OpenEditorAsync(presentable));

        public ICommand removeModelCommand;
        public ICommand RemoveModelCommand => removeModelCommand ??= new DelegateCommand<PresentableData>(async presentable => await RemoveModelAsync(presentable));

        protected override PresentableData CreateModelInstance() => new PresentableNpc();

        protected override Models.Npc CreateResource(PresentableData model) => new Models.Npc() {
            Id = model.Id,
            Name = model.Name
        };
    }
}
