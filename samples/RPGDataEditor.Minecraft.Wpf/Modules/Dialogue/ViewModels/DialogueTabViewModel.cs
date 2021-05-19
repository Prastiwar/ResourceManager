using MediatR;
using Microsoft.Extensions.Logging;
using ResourceManager.Data;

namespace RPGDataEditor.Minecraft.Wpf.Dialogue.ViewModels
{
    public class DialogueTabViewModel : RPGDataEditor.Wpf.Dialogue.ViewModels.DialogueTabViewModel
    {
        public DialogueTabViewModel(IMediator mediator, ILogger<DialogueTabViewModel> logger) : base(mediator, logger) { }

        protected override RPGDataEditor.Models.Dialogue CreateResource(PresentableData model) => new Models.Dialogue() {
            Id = model.Id,
            Title = model.Name,
            Category = (model as PresentableCategoryData).Category
        };

    }
}
