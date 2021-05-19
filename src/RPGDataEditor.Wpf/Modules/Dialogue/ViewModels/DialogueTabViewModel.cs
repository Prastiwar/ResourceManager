using MediatR;
using Microsoft.Extensions.Logging;
using ResourceManager.Data;
using RPGDataEditor.Models;
using RPGDataEditor.Mvvm;

namespace RPGDataEditor.Wpf.Dialogue.ViewModels
{
    public class DialogueTabViewModel : PresentableCategoryDataViewModel<Models.Dialogue>
    {
        public DialogueTabViewModel(IMediator mediator, ILogger<DialogueTabViewModel> logger) : base(mediator, logger) { }

        protected override PresentableData CreateModelInstance() => new PresentableDialogue() { Category = CurrentCategory };

        protected override Models.Dialogue CreateResource(PresentableData model) => new Models.Dialogue() {
            Id = model.Id,
            Title = model.Name,
            Category = (model as PresentableCategoryData).Category
        };
    }
}
